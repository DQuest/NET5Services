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
        /// Получение изображений.
        /// </summary>
        /// <returns></returns>
        [Get("/image/GetAll")]
        IQueryable<ImageModel> GetAll();
        
        /// <summary>
        /// Получение определённого изображения.
        /// </summary>
        /// <param name="id">Id изображения</param>
        /// <returns></returns>
        [Get("/image/Get/{id}")]
        Task<ActionResult<ImageModel>> Get(Guid id);

        /// <summary>
        /// Добавить изображение.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [Post("/image/Create")]
        Task<ActionResult> Create(ImageModel image);

        /// <summary>
        /// Добавить изображения.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [Post("/image/CreateMany")]
        Task<ActionResult> CreateMany(IEnumerable<ImageModel> images);

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [Put("/image/Update")]
        Task<ActionResult> Update(ImageModel image);
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [Put("/image/UpdateMany")]
        Task<ActionResult> UpdateMany(IEnumerable<ImageModel> images);

        /// <summary>
        /// Удалить изображение.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Delete("/image/Delete")]
        Task<ActionResult> Delete(Guid id);

        /// <summary>
        /// Удалить изображения.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Delete("/image/DeleteMany")]
        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}