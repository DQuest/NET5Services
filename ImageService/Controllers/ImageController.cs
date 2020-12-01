using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService ?? throw new ArgumentException(nameof(imageService));
        }

        /// <summary>
        /// Получение изображений для продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns></returns>
        [HttpGet("GetAllImagesForProduct/{productId}")]
        public async Task<IEnumerable<ImageModel>> GetAllImagesForProduct(Guid productId)
        {
            return await _imageService.GetAllImagesForProduct(productId);
        }

        /// <summary>
        /// Получение определённого изображения todo: для продукта.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpGet("GetImages/{imageId}")]
        public async Task<ImageModel> GetImage(Guid imageId)
        {
            return await _imageService.GetImage(imageId);
        }

        /// <summary>
        /// Добавление изображений для продукта.
        /// </summary>
        /// <param name="uploadImagesModel">Модель загрузки изображений для продукта</param>
        /// <returns></returns>
        [HttpPost]
        public async Task UploadImagesForProduct(UploadImagesModel uploadImagesModel)
        {
            await _imageService.UploadImagesForProduct(uploadImagesModel); 
        }

        /// <summary>
        /// Обновление информации об изображении.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateImage(ImageModel image)
        {
            await _imageService.UpdateImage(image); 
        }

        /// <summary>
        /// Удаление изображений для продуктов.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteImages(IEnumerable<Guid> productsIds)
        {
            await _imageService.DeleteImagesForProducts(productsIds);
        }
    }
}