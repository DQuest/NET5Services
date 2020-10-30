using System.Collections.Generic;
using System.Threading.Tasks;
using Homework2.ProductService.Models;
using Refit;

namespace Homework2.ProductService.Clients
{
    public interface IPriceClient
    {
        [Get("/api/prices")]
        Task<IEnumerable<PriceModel>> GetAll();
    }
}