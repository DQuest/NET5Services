using System;
using System.Threading.Tasks;
using CustomImageService.Models;
using Refit;

namespace CustomImageService.Clients
{
    public interface IImageDbClient
    {
        [Get("/api/images/{id}")]
        Task<ImageDbModel> Get(Guid id);

        [Post("/api/images")]
        Task Create(ImageDbModel image);
    }
}