using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceService.Interfaces;
using PriceService.Models;

namespace PriceService.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("[controller]")]
    public class ProductPriceController
    {
        private readonly IProductPriceRepository _productPriceRepository;

        public ProductPriceController(IProductPriceRepository productPriceRepository)
        {
            _productPriceRepository = productPriceRepository;
        }
        
        /// <summary>
        /// Получение цены для продукта.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<PriceModel>> GetPriceForProduct(Guid productId)
        {
            return await _productPriceRepository.GetPriceForProduct(productId);
        }
    }
}