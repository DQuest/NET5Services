using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Homework2.ImageService.Entities;
using Homework2.ImageService.Interfaces;
using Homework2.ImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.ImageService.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImageController(IImageService imageService, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            var imageEntity = await _imageService.GetAll();
            return _mapper.Map<IEnumerable<ImageModel>>(imageEntity);
        }
    }
}