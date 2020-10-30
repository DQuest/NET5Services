using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            return await Task.Run(() => _imageService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ImageModel> Get(long id)
        {
            return await Task.Run(() => _imageService.Get(id));
        }
    }
}