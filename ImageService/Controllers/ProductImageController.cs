using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        
        /// <summary>
        /// Получение изображений для продукта.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<ImageModel>>> GetAllImagesForProduct(Guid productId)
        {
            return await _productImageService.GetAllImagesForProduct(productId);
        }
    }
}