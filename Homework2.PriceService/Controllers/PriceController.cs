using System.Collections.Generic;
using System.Threading.Tasks;
using Homework2.PriceService.Interfaces;
using Homework2.PriceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.PriceService.Controllers
{
    [ApiController]
    [Route("api/prices")]
    public class PriceController : Controller
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<IEnumerable<PriceModel>> GetAll()
        {
            return await Task.Run(() => _priceService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<PriceModel> Get(long id)
        {
            return await Task.Run(() => _priceService.Get(id));
        }
    }
}