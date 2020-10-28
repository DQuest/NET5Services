using System;
using System.Collections.Generic;
using System.Linq;
using Homework2.ProductService.Clients;
using Homework2.ProductService.Interfaces;
using Homework2.ProductService.Models;

namespace Homework2.ProductService.Services
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

            FillProducts();
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

        private void FillProducts()
        {
            _products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = 1,
                    Name = "FirstProduct",
                    Images = _imageClient.GetAll().Where(x => x.Id == 1 || x.Id == 2),
                    Prices = _priceClient.GetAll().Where(x => x.Id == 1 || x.Id == 2)
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "SecondProduct",
                    Images = _imageClient.GetAll().Where(x => x.Id == 3 || x.Id == 4),
                    Prices = _priceClient.GetAll().Where(x => x.Id == 3 || x.Id == 4)
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "ThirdProduct",
                    Images = _imageClient.GetAll().Where(x => x.Id == 5),
                    Prices = _priceClient.GetAll().Where(x => x.Id == 5)
                }
            };
        }
    }
}