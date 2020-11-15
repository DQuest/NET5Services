using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework4.CustomImageService.Interfaces;
using Homework4.CustomImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework4.CustomImageService.Controllers
{
    [ApiController]
    [Route("api/cImages")]
    public class CustomImageController : Controller
    {
        private readonly ICustomImageService _customImageService;

        public CustomImageController(ICustomImageService customImageService)
        {
            _customImageService = customImageService ?? throw new ArgumentException(nameof(customImageService));
        }

        [HttpGet]
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            return await _customImageService.GetAll();
        }

        [HttpGet("GetImageMetaInfo/{id}")]
        public async Task<ImageModel> GetImageMetaInfo(Guid id)
        {
            return await _customImageService.GetImageMetaInfo(id);
        }

        [HttpGet("GetImageUrl/{id}")]
        public async Task<string> GetImageUrl(Guid id)
        {
            return await _customImageService.GetImageUrl(id);
        }

        [HttpPost]
        public async Task Upload(string imageUrl)
        {
            await _customImageService.Upload(imageUrl);
        }
    }
}