using System;
using System.Collections.Generic;
using System.Linq;
using Homework2.ImageService.Interfaces;
using Homework2.PriceService.Interfaces;
using Homework2.ProductService.Interfaces;
using Homework2.ProductService.Models;

namespace Homework2.ProductService.Services
{
    public class ProductService : IProductService
    {
        private IEnumerable<ProductModel> _products;
        
        private readonly IImageService _imageController;
        private readonly IPriceService _priceService;

        public ProductService(
            IImageService imageController,
            IPriceService priceService)
        {
            _imageController = imageController;
            _priceService = priceService;

            FillProducts();
        }

        public IEnumerable<ProductModel> GetAll() => _products;

        public ProductModel Get(long id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new ArgumentException("Нет такого продукта");
            }

            return product;
        }

        private void FillProducts() => _products = new List<ProductModel>
        {
            new ProductModel
            {
                Id = 1,
                Name = "FirstProduct",
                Images = _imageController.GetAll().Where(x => x.Id == 1 || x.Id == 2),
                Prices = _priceService.GetAll().Where(x => x.Id == 1 || x.Id == 2)
            },
            new ProductModel
            {
                Id = 2,
                Name = "SecondProduct",
                Images = _imageController.GetAll().Where(x => x.Id == 3 || x.Id == 4),
                Prices = _priceService.GetAll().Where(x => x.Id == 3 || x.Id == 4)
            },
            new ProductModel
            {
                Id = 3,
                Name = "ThirdProduct",
                Images = _imageController.GetAll().Where(x => x.Id == 5),
                Prices = _priceService.GetAll().Where(x => x.Id == 5)
            }
        };
    }
}