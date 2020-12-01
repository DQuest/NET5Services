using System;

namespace ProductService.Models
{
    public class PriceModel
    {
        public Guid Id { get; set; }
        
        public Guid ProductId { get; set; }

        public decimal SellPrice { get; set; }

        public decimal DiscountPrice { get; set; }
        
        public bool IsLast { get; set; }
    }
}