using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(
            IImageClient imageClient, 
            IPriceClient priceClient, 
            ProductContext productContext, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _imageClient = imageClient ?? throw new ArgumentNullException(nameof(imageClient));
            _priceClient = priceClient ?? throw new ArgumentNullException(nameof(priceClient));
            _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var products = await _productContext.Product
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
            
            if (!products.Any())
            {
                throw new ArgumentException("Продукты не найдены в БД");
            }

            var productsModel = _mapper.Map<IEnumerable<ProductModel>>(products);

            foreach (var product in productsModel)
            {
                try
                {
                    product.Images = await _imageClient.GetAllImagesForProduct(product.Id);
                    product.Prices = await _priceClient.GetActualPriceForProduct(product.Id);
                }
                catch (Exception ex)
                {
                    // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
                }
            }

            return productsModel;
        }

        public async Task<ProductModel> GetProduct(Guid productId)
        {
            var productEntity = await _productContext.Product
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (productEntity == null)
            {
                throw new ArgumentException("Продукт не найден в БД");
            }

            var product = _mapper.Map<ProductModel>(productEntity);
            try
            {
                product.Images = await _imageClient.GetAllImagesForProduct(productId);
                product.Prices = await _priceClient.GetActualPriceForProduct(productId);
            }
            catch (Exception ex)
            {
                // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
            }

            return product;
        }

        public async Task CreateProduct(ProductModel product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            productEntity.Id = new Guid();
            productEntity.CreatedBy = userId;
            productEntity.LastSavedBy = userId;
            productEntity.CreatedDate = DateTime.Now;
            productEntity.LastSavedDate = DateTime.Now;

            await UploadImagesIfExist(product);
            await SetPriceIfExist(product);

            await _productContext.Product.AddRangeAsync(productEntity);
            await _productContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductModel product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            
            productEntity.LastSavedDate = DateTime.Now;

            if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                productEntity.LastSavedBy = userId;
            }

            await UploadImagesIfExist(product);
            await SetPriceIfExist(product);

            _productContext.Product.Update(productEntity);
            await _productContext.SaveChangesAsync();
        }

        public async Task DeleteProducts(IEnumerable<Guid> productsIds)
        {
            var products = await _productContext.Product
                .Where(x => x.IsDeleted == false)
                .Where(x => productsIds.Contains(x.Id))
                .ToListAsync();

            if (!products.Any())
            {
                throw new ArgumentException("Не найдены продукты для удаления");
            }

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            var dateTimeNow = DateTime.Now;
            foreach (var product in products)
            {
                product.LastSavedBy = userId;
                product.LastSavedDate = dateTimeNow;
                product.IsDeleted = true;
            }
            
            // todo в теории, если навешивать ограничение внешнего ключа в БД на дочерние для продукта сущности (изображения и цены)
            // todo то придётся сначала убирать их, перед удаление продукта. Пока ограничения нет, удаление цен и изобрежений не производим

            _productContext.Product.UpdateRange(products);
            await _productContext.SaveChangesAsync();
        }
        
        private async Task SetPriceIfExist(ProductModel product)
        {
            if (product.Prices != null)
            {
                try
                {
                    await _priceClient.SetNewPriceForProduct(new PriceModel
                    {
                        ProductId = product.Id,
                        DiscountPrice = product.Prices.DiscountPrice,
                        SellPrice = product.Prices.SellPrice
                    });
                }
                catch (Exception ex)
                {
                    // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
                }
            }
        }

        private async Task UploadImagesIfExist(ProductModel product)
        {
            if (product.Images.Any())
            {
                try
                {
                    await _imageClient.UploadImagesForProduct(new UploadImagesModel
                    {
                        ProductId = product.Id,
                        ImageUrls = product.Images.Select(x => x.Url)
                    });
                }
                catch (Exception ex)
                {
                    // todo нужна какая-то обёртка, чтобы не сыпать везде try{}catch{} блоки
                }
            }
        }
    }
}