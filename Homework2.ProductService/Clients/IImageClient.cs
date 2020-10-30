using System.Collections.Generic;
using System.Threading.Tasks;
using Homework2.ProductService.Models;
using Refit;

namespace Homework2.ProductService.Clients
{
    public interface IImageClient
    {
        [Get("/api/images")]
        Task<IEnumerable<ImageModel>> GetAll();
    }
}