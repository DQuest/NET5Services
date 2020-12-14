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
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Получение продуктов.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            return _productService.GetAll();
        }

        /// <summary>
        /// Получение определённого продукта.
        /// </summary>
        /// <param name="id">Id продукты</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> Get(Guid id)
        {
            return await _productService.Get(id);
        }

        /// <summary>
        /// Добавить продукт.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(ProductModel product)
        {
            return await _productService.Create(product);
        }

        /// <summary>
        /// Добавить продукты.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        public async Task<ActionResult> CreateMany(IEnumerable<ProductModel> products)
        {
            return await _productService.CreateMany(products);
        }

        /// <summary>
        /// Обновить информацию об продукте.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(ProductModel product)
        {
            return await _productService.Update(product);
        }

        /// <summary>
        /// Обновить информацию об продуктах.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        public async Task<ActionResult> UpdateMany(IEnumerable<ProductModel> products)
        {
            return await _productService.UpdateMany(products);
        }

        /// <summary>
        /// Удалить продукт.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await _productService.Delete(id);
        }

        /// <summary>
        /// Удалить продукты.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMany")]
        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            return await _productService.DeleteMany(ids);
        }
    }
}