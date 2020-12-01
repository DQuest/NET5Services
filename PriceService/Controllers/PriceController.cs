using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceService.Interfaces;
using PriceService.Models;

namespace PriceService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/prices")]
    public class PriceController : Controller
    {
        private readonly IPriceRepository _priceRepository;

        public PriceController(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository ?? throw new ArgumentException(nameof(priceRepository));
        }

        /// <summary>
        /// Получить все цены продукта (даже удалённые).
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PriceModel>> GetAllPricesForProduct(Guid productId)
        {
            return await _priceRepository.GetAllPricesForProduct(productId);
        }

        /// <summary>
        /// Получить актуальную стоимость продукта.
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        [HttpGet("GetActualPriceForProduct/{productId}")]
        public async Task<PriceModel> GetActualPriceForProduct(Guid productId)
        {
            return await _priceRepository.GetActualPriceForProduct(productId);
        }

        /// <summary>
        /// Установить цену продукту.
        /// </summary>
        /// <param name="price">Модель ценника</param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetNewPriceForProduct(PriceModel price)
        {
            await _priceRepository.SetNewPriceForProduct(price);
        }

        /// <summary>
        /// Обновить стоимость.
        /// </summary>
        /// <param name="price">Модель ценника</param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdatePriceForProduct(PriceModel price)
        {
            await _priceRepository.UpdatePriceForProduct(price);
        }

        /// <summary>
        /// Удалить все цены продуктам.
        /// </summary>
        /// <param name="productsIds">Идентификаторы продуктов</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletePricesForProducts(IEnumerable<Guid> productsIds)
        {
            await _priceRepository.DeletePricesForProducts(productsIds);
        }
    }
}