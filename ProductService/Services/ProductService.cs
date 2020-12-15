using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Clients;
using ProductService.Entities;
using ProductService.Interfaces;
using ProductService.Models;
using ProductService.Models.Images;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IImageClient _imageClient;
        private readonly IPriceClient _priceClient;
        private readonly ProductContext _productContext;
        private readonly IMapper _mapper;

        public ProductService(
            IImageClient imageClient,
            IPriceClient priceClient,
            ProductContext productContext,
            IMapper mapper)
        {
            _imageClient = imageClient;
            _priceClient = priceClient;
            _productContext = productContext;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            var products = await _productContext.Product
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync();

            if (!products.Any())
            {
                return new NotFoundObjectResult("Продукты не найдены в БД");
            }

            foreach (var product in products)
            {
                await FillImagesField(product);
                await FillPriceField(product);
            }

            return new ObjectResult(products);
        }

        public async Task<ActionResult<ProductModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Не получен идентификатор продукта");
            }

            var product = await _productContext.Product
                .Where(x => x.Id == id)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDeleted = x.IsDeleted
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return new NotFoundObjectResult("Продукты не найдены в БД");
            }

            await FillImagesField(product);
            await FillPriceField(product);

            return new ObjectResult(product);
        }

        public async Task<ActionResult> Create(ProductModel product)
        {
            var response = new List<string>();
            if (product == null)
            {
                return new NotFoundObjectResult("Отсутствует продукт для добавления");
            }

            var productEntity = _mapper.Map<ProductEntity>(product);

            FillBaseFieldsForCreateAndUpdate(productEntity, true);

            foreach (var image in product.Images)
            {
                image.ProductId = productEntity.Id;
            }

            product.Price.ProductId = productEntity.Id;

            await CreateImagesAndPrice(product, response);

            await _productContext.Product.AddAsync(productEntity);

            return await TryToSaveAndReturnResult(response);
        }

        public async Task<ActionResult> CreateMany(IEnumerable<ProductModel> products)
        {
            var response = new List<string>();
            if (!products.Any())
            {
                return new NotFoundObjectResult("Отсутствуют продукты для добавления");
            }

            foreach (var product in products)
            {
                await CreateImagesAndPrice(product, response);
            }

            var productEntity = _mapper.Map<IEnumerable<ProductEntity>>(products);

            foreach (var product in productEntity)
            {
                FillBaseFieldsForCreateAndUpdate(product, true);
            }

            await _productContext.Product.AddRangeAsync(productEntity);

            return await TryToSaveAndReturnResult(response);
        }

        public async Task<ActionResult> Update(ProductModel product)
        {
            var response = new List<string>();
            if (product == null)
            {
                return new NotFoundObjectResult("Отсутствует продукт для обновления");
            }

            await UpdateImagesAndPrice(product, response);

            var productEntity = _mapper.Map<ProductEntity>(product);

            FillBaseFieldsForCreateAndUpdate(productEntity, false);

            _productContext.Product.Update(productEntity);

            return await TryToSaveAndReturnResult(response);
        }

        public async Task<ActionResult> UpdateMany(IEnumerable<ProductModel> products)
        {
            var response = new List<string>();
            if (!products.Any())
            {
                return new NotFoundObjectResult("Отсутствуют продукты для обновления");
            }

            foreach (var product in products)
            {
                await UpdateImagesAndPrice(product, response);
            }

            var productEntity = _mapper.Map<IEnumerable<ProductEntity>>(products);

            foreach (var product in productEntity)
            {
                FillBaseFieldsForCreateAndUpdate(product, true);
            }

            _productContext.Product.UpdateRange(productEntity);

            return await TryToSaveAndReturnResult(response);
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var response = new List<string>();
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Отсутствует идентификатор для удаления продукта");
            }

            var product = await _productContext.Product
                // .Where(x => x.IsDeleted == false)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return new NotFoundObjectResult("Продукт не найден в БД");
            }

            FillBaseFieldsForDelete(product);
            _productContext.Product.Update(product);

            await DeleteImagesAndPrice(product, response);
            return await TryToSaveAndReturnResult(response);
        }

        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            var response = new List<string>();
            if (!ids.Any())
            {
                return new NotFoundObjectResult("Отсутствуют идентификаторы для удаления продуктов");
            }

            var products = await _productContext.Product
                // .Where(x => x.IsDeleted == false)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (!products.Any())
            {
                return new NotFoundObjectResult("Продукты не найдены в БД");
            }

            foreach (var product in products)
            {
                await DeleteImagesAndPrice(product, response);
                FillBaseFieldsForDelete(product);
            }

            _productContext.Product.UpdateRange(products);
            return await TryToSaveAndReturnResult(response);
        }

        private static void FillBaseFieldsForDelete(ProductEntity product)
        {
            product.LastSavedDate = DateTime.Now;
            product.IsDeleted = true;
        }

        private async Task<ActionResult> TryToSaveAndReturnResult(List<string> response)
        {
            try
            {
                await _productContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                response.Add(exception.Message);
            }

            return new OkObjectResult(string.Join($"{Environment.NewLine}", response));
        }

        // Todo: вынести
        private async Task CreateImagesAndPrice(ProductModel product, List<string> response)
        {
            if (product.Price != null)
            {
                var createPriceResponse = await _priceClient.Create(product.Price);
                if (!createPriceResponse.IsSuccessStatusCode)
                {
                    response.Add(createPriceResponse.Error.Content);
                }
            }

            if (product.Images.Any())
            {
                var createImagesResponse = await _imageClient.CreateMany(product.Images);
                if (!createImagesResponse.IsSuccessStatusCode)
                {
                    response.Add(createImagesResponse.Error.Content);
                }
            }
        }

        private async Task UpdateImagesAndPrice(ProductModel product, List<string> response)
        {
            if (product.Images.Any())
            {
                var updateImagesResponse = await _imageClient.UpdateMany(product.Images);
                if (!updateImagesResponse.IsSuccessStatusCode)
                {
                    response.Add(updateImagesResponse.Error.Content);
                }
            }

            if (product.Price != null)
            {
                var updatePriceResponse = await _priceClient.Update(product.Price);
                if (!updatePriceResponse.IsSuccessStatusCode)
                {
                    response.Add(updatePriceResponse.Error.Content);
                }
            }
        }

        private async Task DeleteImagesAndPrice(ProductEntity product, List<string> response)
        {
            var deleteImageResult = await _imageClient.DeleteImagesForProduct(product.Id);
            if (!deleteImageResult.IsSuccessStatusCode)
            {
                response.Add(deleteImageResult.Error.Content);
            }

            var deletePriceResult = await _priceClient.DeletePriceForProduct(product.Id);
            if (!deletePriceResult.IsSuccessStatusCode)
            {
                response.Add(deletePriceResult.Error.Content);
            }
        }

        private async Task FillImagesField(ProductModel product)
        {
            try
            {
                var imagesForProduct = await _imageClient.GetAllImagesForProduct(product.Id);
                product.Images = imagesForProduct;
            }
            catch (Exception)
            {
                product.Images = new List<ImageModel>();
            }
        }

        private async Task FillPriceField(ProductModel product)
        {
            try
            {
                var priceForProduct = await _priceClient.GetPriceForProduct(product.Id);
                product.Price = priceForProduct;
            }
            catch (Exception)
            {
                product.Price = new PriceModel();
            }
        }

        private void FillBaseFieldsForCreateAndUpdate(ProductEntity productEntity, bool isCreateOperation)
        {
            var now = DateTime.Now;
            if (isCreateOperation)
            {
                productEntity.Id = Guid.NewGuid();
                productEntity.IsDeleted = false;
                productEntity.CreatedDate = now;
            }

            productEntity.LastSavedDate = now;
        }
    }
}