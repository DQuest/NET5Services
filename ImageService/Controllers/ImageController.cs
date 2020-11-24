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

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            var imageEntity = await _imageService.GetAll();
            return _mapper.Map<IEnumerable<ImageModel>>(imageEntity);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ImageModel> Get(Guid id)
        {
            var imageEntity = await _imageService.Get(id);
            return _mapper.Map<ImageModel>(imageEntity);
        }

        [Authorize]
        [HttpPost]
        public async Task Create(ImageModel image)
        {
            // Нужен ли маппинг в entity - модель ?
            var imageEntity = _mapper.Map<ImageEntity>(image);
            await _imageService.Create(imageEntity);
        }

        [Authorize]
        [HttpPut]
        public async Task Update(ImageModel image)
        {
            // Нужен ли маппинг в entity - модель ?
            var imageEntity = _mapper.Map<ImageEntity>(image);
            await _imageService.Update(imageEntity);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _imageService.Delete(id);
        }
    }
}