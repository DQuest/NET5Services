using System;
using BaseRepository;

namespace PriceService.Repositories
{
    public class PriceDbModel : BaseEntity
    {
        public Guid ProductId { get; set; }

        public decimal SellPrice { get; set; }

        public decimal DiscountPrice { get; set; }

        public bool IsLast { get; set; }
    }
}