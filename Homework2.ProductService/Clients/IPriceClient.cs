using System.Collections.Generic;
using Homework2.ProductService.Models;
using Refit;

namespace Homework2.ProductService.Clients
{
    public interface IPriceClient
    {
        [Get("/api/prices")]
        IEnumerable<PriceModel> GetAll();
    }
}