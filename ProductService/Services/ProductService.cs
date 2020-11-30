using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProductService.Clients;
using ProductService.Interfaces;
using ProductService.Models;

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
            _imageClient = imageClient ?? throw new ArgumentNullException(nameof(imageClient));
            _priceClient = priceClient ?? throw new ArgumentNullException(nameof(priceClient));
            _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public async Task<IEnumerable<ProductModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductModel> Get(Guid productId)
        {
            var productEntity = _productContext.Product
                .Where(x => x.IsDeleted == false)
                .FirstOrDefault(x => x.Id == productId);

            if (productEntity == null)
            {
                throw new ArgumentException("Продукт не найден");
            }

            var product = _mapper.Map<ProductModel>(productEntity);
            product.Images = await _imageClient.GetAll(productId);
            product.Prices = await _priceClient.Get(productId);

            return product;
        }

        public async Task Create(ProductModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task Update(ProductModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}