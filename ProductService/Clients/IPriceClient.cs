using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models;
using Refit;

namespace ProductService.Clients
{
    public interface IPriceClient
    {
        [Get("/api/prices/GetPriceForProduct/{productId}")]
        Task<PriceModel> Get(Guid productId);
    }
}