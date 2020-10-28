using System.Collections.Generic;
using Homework2.ProductService.Interfaces;
using Homework2.ProductService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.ProductService.Controllers
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<ProductModel> GetAll()
        {
            return _productService.GetAll();
        }

        [HttpGet("{id}")]
        public ProductModel Get(long id)
        {
            return _productService.Get(id);
        }
    }
}