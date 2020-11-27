using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models;
using Refit;

namespace ProductService.Clients
{
    public interface IImageClient
    {
        [Get("/api/images/GetAllImages/{productId}")]
        Task<IEnumerable<ImageModel>> GetAll(Guid productId);
    }
}