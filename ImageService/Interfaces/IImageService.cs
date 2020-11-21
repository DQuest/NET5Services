using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Entities;

namespace ImageService.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<ImageEntity>> GetAll();

        Task<ImageEntity> Get(Guid id);

        Task Create(ImageEntity entity);

        Task Update(ImageEntity entity);

        Task Delete(Guid id);
    }
}