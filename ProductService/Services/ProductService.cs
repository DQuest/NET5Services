using System;
using System.Collections.Generic;
using System.Linq;
using ProductService.Clients;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private IEnumerable<ProductModel> _products;

        private readonly IImageClient _imageClient;
        private readonly IPriceClient _priceClient;

        public ProductService(IImageClient imageClient, IPriceClient priceClient)
        {
            _imageClient = imageClient ?? throw new ArgumentNullException(nameof(imageClient));
            _priceClient = priceClient ?? throw new ArgumentNullException(nameof(priceClient));
        }

        public IEnumerable<ProductModel> GetAll()
        {
            return _products;
        }

        public ProductModel Get(long id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new ArgumentException("Нет такого продукта");
            }

            return product;
        }
    }
}