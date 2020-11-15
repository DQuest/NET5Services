using System;
using System.Threading.Tasks;
using Homework4.CustomImageService.Models;
using Refit;

namespace Homework4.CustomImageService.Clients
{
    public interface IImageDbClient
    {
        [Get("/api/images/{id}")]
        Task<ImageDbModel> Get(Guid id);

        [Post("/api/images")]
        Task Create(ImageDbModel image);
    }
}