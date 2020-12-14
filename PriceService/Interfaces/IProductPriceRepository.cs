using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceService.Models;

namespace PriceService.Interfaces
{
    public interface IProductPriceRepository
    {
        Task<ActionResult<PriceModel>> GetPriceForProduct(Guid productId);
    }
}