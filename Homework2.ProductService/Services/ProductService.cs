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
       // private IEnumerable<ImageModel> _images;
        
        private readonly IImageClient _imageClient;

        public ProductService(IImageClient imageClient)
        {
            _imageClient = imageClient;

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

        private void FillProducts()
        {
            var test = _imageClient.GetAll();

            _products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = 1,
                    Name = "FirstProduct",
                    Images = _imageClient.GetAll().Where(x => x.Id == 1 || x.Id == 2),
                    /*Prices = _priceService.GetAll().Where(x => x.Id == 1 || x.Id == 2)*/
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "SecondProduct",
                    /*Images = _imageService.GetAll().Where(x => x.Id == 3 || x.Id == 4),
                    Prices = _priceService.GetAll().Where(x => x.Id == 3 || x.Id == 4)*/
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "ThirdProduct",
                    /*Images = _imageService.GetAll().Where(x => x.Id == 5),
                    Prices = _priceService.GetAll().Where(x => x.Id == 5)*/
                }
            };
        }
    }
}