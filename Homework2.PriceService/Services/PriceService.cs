using System;
using System.Collections.Generic;
using System.Linq;
using Homework2.PriceService.Interfaces;
using Homework2.PriceService.Models;

namespace Homework2.PriceService.Services
{
    public class PriceService : IPriceService
    {
        private IEnumerable<PriceModel> _prices;

        public PriceService()
        {
            FillPrices();
        }

        public IEnumerable<PriceModel> GetAll()
        {
            return _prices;
        }

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