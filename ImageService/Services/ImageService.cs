using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImageService.Clients;
using ImageService.Entities;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageContext _imageContext;
        private readonly IYandexDriveImageClient _yandexDriveImageClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(ImageContext imageContext, IYandexDriveImageClient yandexDriveImageClient, IHttpContextAccessor httpContextAccessor)
        {
            _imageContext = imageContext ?? throw new ArgumentException(nameof(imageContext));
            _yandexDriveImageClient = yandexDriveImageClient ?? throw new ArgumentException(nameof(yandexDriveImageClient));;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<ImageEntity>> GetAll(Guid productId)
        {
            var images = await _imageContext.Image
                .Where(x => x.ProductId == productId)
                .ToListAsync();
            
            if (images == null)
            {
                throw new ArgumentException($"Изображения для продукта с идентификатором {productId} не найдены.");
            }

            return images;
        }

        public async Task<ImageEntity> Get(Guid imageId)
        {
            var image = await _imageContext.Image.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image == null)
            {
                throw new ArgumentException($"Не удалось получить увеличенное изображение с идентификатором {imageId}.");
            }
            
            // todo менять size=S в ссылке для отображения увеличенного изображения. Например на size=XL

            return image;
        }

        public async Task Create(UploadImagesModel uploadImagesModel)
        {
            // todo заливка изображения на ядиск, ссылку на превьюшку в БД
            try
            {
                var fullPath = GetFullPathForImage(imageUrl);
                var response = await _yandexDriveImageClient.Upload(imageUrl, fullPath, _token);
                var deserializedResponse = JsonConvert.DeserializeObject<UploadResponseModel>(response);

                await _imageDbClient.Create(new ImageDbModel
                {
                    Id = new Guid(),
                    Url = deserializedResponse.Href,
                    FullPathOnDisk = fullPath,
                    ProductId = Guid.Parse("00000000-0000-0000-0000-000000000000")
                });
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            
            var dateTimeNow = DateTime.UtcNow;
            
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            var images = uploadImagesModel.ImageUrls.Select(imageUrl => new ImageEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = dateTimeNow,
                    LastSavedDate = dateTimeNow,
                    CreatedBy = userId,
                    LastSavedBy = userId,
                    ProductId = uploadImagesModel.ProductId,
                    PreviewUrl = imageUrl
                })
                .ToList();

            await _imageContext.Image.AddRangeAsync(images);
            await _imageContext.SaveChangesAsync();
        }

        public async Task Update(ImageEntity imageEntity)
        {
            imageEntity.LastSavedDate = DateTime.UtcNow;

            if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                imageEntity.LastSavedBy = userId;
            }

            _imageContext.Image.Update(imageEntity);
            await _imageContext.SaveChangesAsync();
        }

        public async Task Delete(IEnumerable<Guid> productsIds)
        {
            var productsImages = await _imageContext.Image
                .Where(x => productsIds.Contains(x.ProductId))
                .ToListAsync();

            if (productsImages == null)
            {
                throw new ArgumentException($"Не найдено изображений для продуктов с идентификаторами: {string.Join(", ", productsIds)}");
            }

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            var dateTimeNow = DateTime.Now;
            foreach (var image in productsImages)
            {
                image.LastSavedBy = userId;
                image.LastSavedDate = dateTimeNow;
                image.IsDeleted = true;
            }

            _imageContext.Image.RemoveRange(productsImages);
            await _imageContext.SaveChangesAsync();
        }
    }
}