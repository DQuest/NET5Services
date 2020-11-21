using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImageService.Entities;
using ImageService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageContext _imageContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(ImageContext imageContext, IHttpContextAccessor httpContextAccessor)
        {
            _imageContext = imageContext ?? throw new ArgumentException(nameof(imageContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<ImageEntity>> GetAll()
        {
            return await _imageContext.Image.ToListAsync();
        }

        public async Task<ImageEntity> Get(Guid id)
        {
            var image = await _imageContext.Image.FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                throw new ArgumentException($"Изображение с идентификатором {id} не найдено в БД.");
            }

            return image;
        }

        public async Task Create(ImageEntity entity)
        {
            // Guid.Empty - "00000000-0000-0000-0000-000000000000"
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

            if (imageEntity == null)
            {
                throw new ArgumentException($"Не найдено изображение с {id} в БД");
            }

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                imageEntity.LastSavedBy = userId;
            }

            _imageContext.Image.Remove(imageEntity);
            await _imageContext.SaveChangesAsync();
        }
    }
}