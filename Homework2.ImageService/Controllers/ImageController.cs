using System.Collections.Generic;
using Homework2.ImageService.Interfaces;
using Homework2.ImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.ImageService.Controllers
{
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public IEnumerable<ImageModel> GetAll()
        {
            return _imageService.GetAll();
        }

        [HttpGet("{id}")]
        public ImageModel Get(long id)
        {
            return _imageService.Get(id);
        }
    }
}