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

        // public async async Task<IEnumerable<ProductModel>> GetAll()
        // {
        //     var products = await _productContext.Product
        //         .Where(x => x.IsDeleted == false)
        //         .ToListAsync();
        //
        //     if (!products.Any())
        //     {
        //         throw new ArgumentException("Продукты не найдены в БД");
        //     }
        //
        //     var productsModel = _mapper.Map<IEnumerable<ProductModel>>(products);
        //
        //     foreach (var product in productsModel)
        //     {
        //         try
        //         {
        //             product.Images = _imageClient.GetAll().Where(x => x.ProductId == product.Id);
        //             product.Prices = _priceClient.GetAll();
        //         }
        //         catch (Exception ex)
        //         {
        //             // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
        //         }
        //     }
        //
        //     return productsModel;
        // }
        //
        // public async async Task<ProductModel> Get(Guid productId)
        // {
        //     var productEntity = await _productContext.Product
        //         .Where(x => x.IsDeleted == false)
        //         .FirstOrDefaultAsync(x => x.Id == productId);
        //
        //     if (productEntity == null)
        //     {
        //         throw new ArgumentException("Продукт не найден в БД");
        //     }
        //
        //     var product = _mapper.Map<ProductModel>(productEntity);
        //     try
        //     {
        //         product.Images = await _imageClient.GetAllImagesForProduct(productId);
        //         product.Prices = await _priceClient.GetActualPriceForProduct(productId);
        //     }
        //     catch (Exception ex)
        //     {
        //         // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
        //     }
        //
        //     return product;
        // }
        //
        // public async async Task CreateProduct(ProductModel product)
        // {
        //     var productEntity = _mapper.Map<ProductEntity>(product);
        //
        //     Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        //         out var userId);
        //
        //     productEntity.Id = product.Id = new Guid();
        //     productEntity.CreatedBy = userId;
        //     productEntity.LastSavedBy = userId;
        //     productEntity.CreatedDate = DateTime.Now;
        //     productEntity.LastSavedDate = DateTime.Now;
        //
        //     await UploadImagesIfExist(product);
        //     await SetPriceIfExist(product);
        //
        //     await _productContext.Product.AddRangeAsync(productEntity);
        //     await _productContext.SaveChangesAsync();
        // }
        //
        // public async async Task UpdateProduct(ProductModel product)
        // {
        //     var productEntity = _mapper.Map<ProductEntity>(product);
        //
        //     productEntity.LastSavedDate = DateTime.Now;
        //
        //     if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        //         out var userId))
        //     {
        //         productEntity.LastSavedBy = userId;
        //     }
        //
        //     await UploadImagesIfExist(product);
        //     await SetPriceIfExist(product);
        //
        //     _productContext.Product.Update(productEntity);
        //     await _productContext.SaveChangesAsync();
        // }
        //
        // public async async Task DeleteProducts(IEnumerable<Guid> productsIds)
        // {
        //     var products = await _productContext.Product
        //         .Where(x => x.IsDeleted == false)
        //         .Where(x => productsIds.Contains(x.Id))
        //         .ToListAsync();
        //
        //     if (!products.Any())
        //     {
        //         throw new ArgumentException("Не найдены продукты для удаления");
        //     }
        //
        //     Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        //         out var userId);
        //
        //     var dateTimeNow = DateTime.Now;
        //     foreach (var product in products)
        //     {
        //         product.LastSavedBy = userId;
        //         product.LastSavedDate = dateTimeNow;
        //         product.IsDeleted = true;
        //     }
        //
        //     // todo в теории, если навешивать ограничение внешнего ключа в БД на дочерние для продукта сущности (изображения и цены)
        //     // todo то придётся сначала убирать их, перед удаление продукта. Пока ограничения нет, удаление цен и изобрежений не производим
        //
        //     _productContext.Product.UpdateRange(products);
        //     await _productContext.SaveChangesAsync();
        // }
        //
        // private async async Task SetPriceIfExist(ProductModel product)
        // {
        //     if (product.Prices != null)
        //     {
        //         try
        //         {
        //             await _priceClient.SetNewPriceForProduct(new PriceModel
        //             {
        //                 ProductId = product.Id,
        //                 DiscountPrice = product.Prices.DiscountPrice,
        //                 SellPrice = product.Prices.SellPrice
        //             });
        //         }
        //         catch (Exception ex)
        //         {
        //             // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
        //         }
        //     }
        // }
        //
        // private async async Task UploadImagesIfExist(ProductModel product)
        // {
        //     if (product.Images.Any())
        //     {
        //         try
        //         {
        //             await _imageClient.UploadImagesForProduct(new UploadImagesModel
        //             {
        //                 ProductId = product.Id,
        //                 ImageUrls = product.Images.Select(x => x.Url)
        //             });
        //         }
        //         catch (Exception ex)
        //         {
        //             // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
        //         }
        //     }
        // }
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            var products = await _productContext.Product
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
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
            var product = await _productContext.Product
                .Where(x => x.Id == id)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
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

            await CreateImagesAndPrice(product, response);

            var productEntity = _mapper.Map<ProductEntity>(product);

            FillBaseFields(productEntity, true);

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
                FillBaseFields(product, true);
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

            FillBaseFields(productEntity, false);

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
                FillBaseFields(product, true);
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
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return new NotFoundObjectResult("Продукт не найден в БД");
            }

            await DeleteImagesAndPrice(product, response);

            FillBaseFields(product, false);
            product.IsDeleted = true;

            _productContext.Product.Update(product);

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
                .Where(x => x.IsDeleted == false)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (!products.Any())
            {
                return new NotFoundObjectResult("Продукты не найдены в БД");
            }

            foreach (var product in products)
            {
                await DeleteImagesAndPrice(product, response);
                FillBaseFields(product, false);
                product.IsDeleted = true;
            }

            _productContext.Product.UpdateRange(products);

            return await TryToSaveAndReturnResult(response);
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
                try
                {
                    await _priceClient.Create(product.Price);
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
            }

            if (product.Images.Any())
            {
                try
                {
                    await _imageClient.CreateMany(product.Images);
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
            }
        }

        private async Task UpdateImagesAndPrice(ProductModel product, List<string> response)
        {
            if (product.Images.Any())
            {
                try
                {
                    await _imageClient.UpdateMany(product.Images);
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
            }

            if (product.Price != null)
            {
                try
                {
                    await _priceClient.Update(product.Price);
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
            }
        }

        private async Task DeleteImagesAndPrice(ProductEntity product, List<string> response)
        {
            var images = await _imageClient.GetAllImagesForProduct(product.Id);
            if (images.Any())
            {
                try
                {
                    await _imageClient.DeleteMany(images.Select(x => x.Id));
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
            }

            var price = await _priceClient.GetPriceForProduct(product.Id);
            if (price == null)
            {
                try
                {
                    await _imageClient.Delete(price.Id);
                }
                catch (Exception exception)
                {
                    response.Add(exception.Message);
                }
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

        private void FillBaseFields(ProductEntity productEntity, bool isCreateOperation)
        {
            var now = DateTime.Now;
            if (isCreateOperation)
            {
                productEntity.IsDeleted = false;
                productEntity.CreatedDate = now;
            }

            productEntity.LastSavedDate = now;
        }
    }
}