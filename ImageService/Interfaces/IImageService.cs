using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Models;

namespace ImageService.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<ImageModel>> GetAllImagesForProduct(Guid productId);

        Task<ImageModel> GetImage(Guid imageId);

        Task UploadImagesForProduct(UploadImagesModel uploadImagesModel);

        Task UpdateImage(ImageModel image);

        Task DeleteImagesForProducts(IEnumerable<Guid> productsIds);
    }
}