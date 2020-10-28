namespace Homework2.Services.PriceService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/prices")]
    public class PriceController : Controller, IPriceController
    {
        private IEnumerable<PriceModel> _prices;

        public PriceController()
        {
            FillPrices();
        }

        [HttpGet]
        public IEnumerable<PriceModel> GetAll()
        {
            return _prices;
        }    
        
        [HttpGet("{id}")]
        public PriceModel Get(long id)
        {
            var price = _prices.FirstOrDefault(x => x.Id == id);

            if (price == null)
            {
                throw new ArgumentException("Нет такого ценника");
            }

            return price;
        } 

        private void FillPrices()
        {
            _prices = new List<PriceModel>
            {
                new PriceModel {Id = 1, DiscountPrice = 1000, SellPrice = 1500},
                new PriceModel {Id = 2, DiscountPrice = 1500, SellPrice = 2000},
                new PriceModel {Id = 3, DiscountPrice = 2000, SellPrice = 2500},
                new PriceModel {Id = 4, DiscountPrice = 2500, SellPrice = 3000},
                new PriceModel {Id = 5, DiscountPrice = 3000, SellPrice = 3500}
            };
        }
    }
}