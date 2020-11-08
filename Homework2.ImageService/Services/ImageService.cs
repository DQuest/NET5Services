using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Homework2.ImageService.Entities;
using Homework2.ImageService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Homework2.ImageService.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageContext _imageContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(ImageContext imageContext, IHttpContextAccessor httpContextAccessor)
        {
            _imageContext = imageContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ImageEntity>> GetAll()
        {
            return await _imageContext.Image.ToListAsync();
        }

        public async Task<ImageEntity> Get(Guid id)
        {
            return await _imageContext.Image.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(ImageEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.LastSavedDate = DateTime.UtcNow;

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                entity.CreatedBy = userId;
                entity.LastSavedBy = userId;
            }
            
            await _imageContext.Image.AddAsync(entity);
            await _imageContext.SaveChangesAsync();
        }

        public async Task Update(ImageEntity entity)
        {
            entity.LastSavedDate = DateTime.UtcNow;

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                entity.LastSavedBy = userId;
            }
            
            _imageContext.Image.Update(entity);
            await _imageContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var imageEntity = await _imageContext.Image.FirstOrDefaultAsync(x => x.Id == id);

            if (imageEntity != null)
            {
                _imageContext.Image.Remove(imageEntity);
                await _imageContext.SaveChangesAsync();
            }
        }
    }
}