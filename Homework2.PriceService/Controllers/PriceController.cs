using System.Collections.Generic;
using Homework2.PriceService.Interfaces;
using Homework2.PriceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.PriceService.Controllers
{
    [Route("api/prices")]
    public class PriceController : Controller
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet]
        public IEnumerable<PriceModel> GetAll() =>
            _priceService.GetAll();

        [HttpGet("{id}")]
        public PriceModel Get(long id) =>
            _priceService.Get(id);
    }
}