using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.Images;
using Refit;

namespace ProductService.Clients
{
    public interface IImageClient
    {
        /// <summary>
        /// Получение изображений для продукта.
        /// </summary>
        /// <returns></returns>
        [Get("/ProductImage/{productId}")]
        Task<IEnumerable<ImageModel>> GetAllImagesForProduct(Guid productId);
        
        /// <summary>
        /// Получение определённого изображения.
        /// </summary>
        /// <param name="id">Id изображения</param>
        /// <returns></returns>
        [Get("/Image/{id}")]
        Task<ImageModel> Get(Guid id);

        /// <summary>
        /// Добавить изображение.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [Post("/Image")]
        Task<ActionResult> Create(ImageModel image);

        /// <summary>
        /// Добавить изображения.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [Post("/Image/CreateMany")]
        Task<ActionResult> CreateMany(IEnumerable<ImageModel> images);

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [Put("/Image")]
        Task<ActionResult> Update(ImageModel image);
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [Put("/Image/UpdateMany")]
        Task<ActionResult> UpdateMany(IEnumerable<ImageModel> images);

        /// <summary>
        /// Удалить изображение.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Delete("/Image")]
        Task<ActionResult> Delete(Guid id);

        /// <summary>
        /// Удалить изображения.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Delete("/Image/DeleteMany")]
        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}