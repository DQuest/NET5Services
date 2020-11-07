using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework2.ImageService.Entities;
using Homework2.ImageService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homework2.ImageService.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageContext _imageContext;
        
        public ImageService(ImageContext imageContext)
        {
            _imageContext = imageContext;
        }
        
        public async Task<IEnumerable<ImageEntity>> GetAll()
        {
            return await _imageContext.Image.ToListAsync();
        }
    }
}