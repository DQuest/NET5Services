using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Entities;
using ImageService.Models;

namespace ImageService.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<ImageEntity>> GetAll(Guid productId);

        Task<ImageEntity> Get(Guid imageId);

        Task Create(UploadImagesModel uploadImagesModel);

        Task Update(ImageEntity imageEntity);

        Task Delete(IEnumerable<Guid> productsIds);
    }
}