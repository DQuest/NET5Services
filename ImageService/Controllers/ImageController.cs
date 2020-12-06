using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageService.Entities;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService ?? throw new ArgumentException(nameof(imageService));
        }

        /// <summary>
        /// Получение изображений.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ImageEntity> GetAll()
        {
            return _imageService.GetAll();
        }
        
        /// <summary>
        /// Получение определённого изображения.
        /// </summary>
        /// <param name="id">Id изображения</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageModel>> Get(Guid id)
        {
            return await _imageService.Get(id);
        }

        /// <summary>
        /// Добавить изображение.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(ImageModel image)
        {
            return await _imageService.Create(image);
        }

        /// <summary>
        /// Добавить изображения.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        public async Task<ActionResult> CreateMany(IEnumerable<ImageModel> images)
        {
            return await _imageService.CreateMany(images);
        }

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(ImageModel image)
        {
            return await _imageService.Update(image);
        }
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        public async Task<ActionResult> UpdateMany(IEnumerable<ImageModel> images)
        {
            return await _imageService.UpdateMany(images);
        }

        /// <summary>
        /// Удалить изображение.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await _imageService.Delete(id);
        }

        /// <summary>
        /// Удалить изображения.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMany")]
        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            return await _imageService.DeleteMany(ids);
        }
    }
}