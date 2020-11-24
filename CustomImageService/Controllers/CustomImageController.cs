using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomImageService.Interfaces;
using CustomImageService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomImageService.Controllers
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

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            return await _customImageService.GetAll();
        }

        [Authorize]
        [HttpGet("GetImageMetaInfo/{id}")]
        public async Task<ImageModel> GetImageMetaInfo(Guid id)
        {
            return await _customImageService.GetImageMetaInfo(id);
        }

        [Authorize]
        [HttpGet("GetImageUrl/{id}")]
        public async Task<string> GetImageUrl(Guid id)
        {
            return await _customImageService.GetImageUrl(id);
        }

        [Authorize]
        [HttpPost]
        public async Task Upload(string imageUrl)
        {
            await _customImageService.Upload(imageUrl);
        }
    }
}