namespace Homework2.Services.ProductService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Homework2.Services.ImageService;
    using Homework2.Services.PriceService;
    using Microsoft.AspNetCore.Mvc;

    [Route("product")]
    public class ProductController : Controller, IProductController
    {
        private IEnumerable<ProductModel> _products;
        
        private readonly IImageController _imageController;
        private readonly IPriceController _priceController;

        public ProductController(
            IImageController imageController,
            IPriceController priceController)
        {
            _imageController = imageController;
            _priceController = priceController;

            FillProducts();
        }

        [HttpGet]
        public IEnumerable<ProductModel> GetAll()
        {
            return _products;
        }

        [HttpGet("{id}")]
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
                    Images = _imageController.GetAll().Where(x => x.Id == 1 || x.Id == 2),
                    Prices = _priceController.GetAll().Where(x => x.Id == 1 || x.Id == 2)
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "SecondProduct",
                    Images = _imageController.GetAll().Where(x => x.Id == 3 || x.Id == 4),
                    Prices = _priceController.GetAll().Where(x => x.Id == 3 || x.Id == 4)
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "ThirdProduct",
                    Images = _imageController.GetAll().Where(x => x.Id == 5),
                    Prices = _priceController.GetAll().Where(x => x.Id == 5)
                }
            };
        }
    }
}