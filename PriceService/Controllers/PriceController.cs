using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceService.Interfaces;
using PriceService.Models;

namespace PriceService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PriceController : Controller
    {
        private readonly IPriceRepository _priceRepository;

        public PriceController(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository ?? throw new ArgumentException(nameof(priceRepository));
        }

        /// <summary>
        /// Получение всех цен.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceModel>>> GetAll()
        {
            return await _priceRepository.GetAll();
        }
        
        /// <summary>
        /// Получение стоимости.
        /// </summary>
        /// <param name="id">Id ценника</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceModel>> Get(Guid id)
        {
            return await _priceRepository.Get(id);
        }

        /// <summary>
        /// Добавить стоимость.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(PriceModel price)
        {
            return await _priceRepository.Create(price);
        }

        /// <summary>
        /// Добавить цены.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        public async Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices)
        {
            return await _priceRepository.CreateMany(prices);
        }

        /// <summary>
        /// Обновить стоимость.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(PriceModel price)
        {
            return await _priceRepository.Update(price);
        }
        
        /// <summary>
        /// Обновить цены.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        public async Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices)
        {
            return await _priceRepository.UpdateMany(prices);
        }

        /// <summary>
        /// Удалить стоимость.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await _priceRepository.Delete(id);
        }

        /// <summary>
        /// Удалить цены.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMany")]
        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            return await _priceRepository.DeleteMany(ids);
        }
    }
}