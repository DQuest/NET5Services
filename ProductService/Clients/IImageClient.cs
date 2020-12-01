using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models.Images;
using Refit;

namespace ProductService.Clients
{
    public interface IImageClient
    {
        /// <summary>
        /// Получение изображений для продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns></returns>
        [Get("/api/images/GetAllImagesForProduct/{productId}")]
        Task<IEnumerable<ImageModel>> GetAllImagesForProduct(Guid productId);

        /// <summary>
        /// Добавление изображений для продукта.
        /// </summary>
        /// <param name="uploadImagesModel">Модель загрузки изображений для продукта</param>
        /// <returns></returns>
        [Post("/api/images")]
        Task UploadImagesForProduct(UploadImagesModel uploadImagesModel);

        /// <summary>
        /// Удаление изображений для продуктов.
        /// </summary>
        /// <returns></returns>
        [Delete("/api/images")]
        Task DeleteImages(IEnumerable<Guid> productsIds);
    }
}