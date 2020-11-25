using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageService.Clients;
using ImageService.Entities;
using ImageService.Interfaces;
using ImageService.Models;
using ImageService.Models.YandexDisk;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ImageService.Services
{
    public class ImageService : IImageService
    {
        private readonly string _token;
        private readonly ImageContext _imageContext;
        private readonly IYandexDiskImageClient _yandexDiskImageClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(
            ImageContext imageContext, 
            IYandexDiskImageClient yandexDiskImageClient, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration cfg)
        {
            _token = cfg.GetValue<string>("YandexToken");
            _imageContext = imageContext ?? throw new ArgumentException(nameof(imageContext));
            _yandexDiskImageClient = yandexDiskImageClient ?? throw new ArgumentException(nameof(yandexDiskImageClient));;
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

            return image;
        }

        public async Task Create(UploadImagesModel uploadImagesModel)
        {
            var dateTimeNow = DateTime.UtcNow;

            var imagesHrefs = await UploadImagesToYandexDisk(uploadImagesModel.ImageUrls);

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            var images = imagesHrefs.Select(imageUrl => new ImageEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = dateTimeNow,
                    LastSavedDate = dateTimeNow,
                    CreatedBy = userId,
                    LastSavedBy = userId,
                    ProductId = uploadImagesModel.ProductId,
                    Url = imageUrl
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
        
        private async Task<List<string>> UploadImagesToYandexDisk(IEnumerable<string> imagesUrls)
        {
            var imagesHrefs = new List<string>();
            try
            {
                foreach (var imageUrl in imagesUrls)
                {
                    var fullPath = GetFullPathForImage(imageUrl);
                    var response = await _yandexDiskImageClient.Upload(imageUrl, fullPath, _token);
                    var deserializedResponse = JsonConvert.DeserializeObject<UploadResponse>(response);

                    imagesHrefs.Add(deserializedResponse.Href);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return imagesHrefs;
        }

        /// <summary>
        /// Сформировать полный путь для заливки файла в папку CustomImageFolder 
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        private string GetFullPathForImage(string imageUrl)
        {
            // Вытягиваем регулярками название изображения с расширением
            var imgNameWithExtensionPattern = new Regex(@"[\w-]+\.(jpg|jpeg|png|bmp|gif)");
            var imgNameWithExtension = imgNameWithExtensionPattern.Match(imageUrl).Value;

            // Если у файла нет расшриения и названия, льём с названием "image" без расширения
            if (string.IsNullOrEmpty(imgNameWithExtension))
            {
                imgNameWithExtension = "image";
            }

            return $"CustomImageFolder/{imgNameWithExtension}";
        }
    }
}