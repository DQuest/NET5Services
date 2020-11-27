using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductModel>> GetAll()
        {
            return await _productService.GetAll();
        }

        [HttpGet("{productId}")]
        public async Task<ProductModel> Get(Guid productId)
        {
            return await _productService.Get(productId);
        }

        [HttpPost]
        public async Task Create(ProductModel product)
        {
            await _productService.Create(product);
        }

        [HttpPut]
        public async Task Update(ProductModel product)
        {
            await _productService.Update(product);
        }

        [HttpDelete("{productId}")]
        public async Task Delete(Guid productId)
        {
            await _productService.Delete(productId);
        }
    }
}