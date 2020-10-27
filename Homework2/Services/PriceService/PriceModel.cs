namespace Homework2.Services.PriceService
{
    using System;

    public class PriceModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Стандартная цена.
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// Цена для распродажи.
        /// </summary>
        public decimal DiscountPrice { get; set; }
    }
}