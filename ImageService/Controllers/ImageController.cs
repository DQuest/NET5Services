using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ImageService.Entities;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ImageController(IMapper mapper, IImageService imageService)
        {
            _mapper = mapper;
            _imageService = imageService;
        }

        /// <summary>
        /// Получение изображений для продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllImages/{productId}")]
        public async Task<IEnumerable<ImageModel>> GetAll(Guid productId)
        {
            var imagesEntity = await _imageService.GetAll(productId);
            return _mapper.Map<IEnumerable<ImageModel>>(imagesEntity);
        }

        /// <summary>
        /// Получение определённого изображения todo: для продукта.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetImages/{imageId}")]
        public async Task<ImageModel> Get(Guid imageId)
        {
            var imageEntity = await _imageService.Get(imageId);
            return _mapper.Map<ImageModel>(imageEntity);
        }

        /// <summary>
        /// Добавление изображений для продукта.
        /// </summary>
        /// <param name="uploadImagesModel">Модель загрузки изображений для продукта</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task Create(UploadImagesModel uploadImagesModel)
        {
            await _imageService.Create(uploadImagesModel); 
        }

        /// <summary>
        /// Обновление информации об изображении.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task Update(ImageModel image)
        {
            var imageEntity = _mapper.Map<ImageEntity>(image);
            await _imageService.Update(imageEntity); 
        }

        /// <summary>
        /// Удаление изображений для продуктов.
        /// </summary>
        /// <param name="productsIds">Идентификаторы продуктов</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{productsIds}")]
        public async Task Delete(IEnumerable<Guid> productsIds)
        {
            await _imageService.Delete(productsIds);
        }
    }
}