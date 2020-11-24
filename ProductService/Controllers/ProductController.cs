using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<ProductModel> GetAll()
        {
            return _productService.GetAll();
        }

        [Authorize]
        [HttpGet("{id}")]
        public ProductModel Get(long id)
        {
            return _productService.Get(id);
        }
    }
}