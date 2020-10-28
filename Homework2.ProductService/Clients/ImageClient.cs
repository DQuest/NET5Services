using System.Collections.Generic;
using Homework2.ProductService.Models;
using Refit;

namespace Homework2.ProductService.Clients
{
    public interface IImageClient
    {
        [Get("/api/images")]
        IEnumerable<ImageModel> GetAll();
    }
}