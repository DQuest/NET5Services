using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));;
        }

        /// <summary>
        /// Получить все продукты.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return await _productService.GetAllProducts();
        }

        /// <summary>
        /// Получить продукт.
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<ProductModel> GetProduct(Guid productId)
        {
            return await _productService.GetProduct(productId);
        }

        /// <summary>
        /// Добавить продукт.
        /// </summary>
        /// <param name="product">Модель продукта</param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateProduct(ProductModel product)
        {
            await _productService.CreateProduct(product);
        }

        /// <summary>
        /// Изменить продукт.
        /// </summary>
        /// <param name="product">Модель продукта</param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProduct(ProductModel product)
        {
            await _productService.UpdateProduct(product);
        }

        /// <summary>
        /// Удалить продукты.
        /// </summary>
        /// <param name="productsIds">Идентификаторы продуктов</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteProducts(IEnumerable<Guid> productsIds)
        {
            await _productService.DeleteProducts(productsIds);
        }
    }
}