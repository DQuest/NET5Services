using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ImageService.Clients;
using ImageService.Entities;
using ImageService.Interfaces;
using ImageService.Models;
using ImageService.Models.YandexDisk;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public ImageService(
            ImageContext imageContext,
            IYandexDiskImageClient yandexDiskImageClient,
            IConfiguration cfg,
            IMapper mapper)
        {
            _token = cfg.GetValue<string>("YandexToken");
            _imageContext = imageContext;
            _yandexDiskImageClient = yandexDiskImageClient;
            _mapper = mapper;
        }
        
        public async Task<ActionResult<ImageModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Отсутствует идентификатор изображения");
            }

            var image = await _imageContext.Image
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                return new NotFoundObjectResult("Изображение не найдено в БД");
            }

            return _mapper.Map<ImageModel>(image);
        }

        public IQueryable<ImageModel> GetAll()
        {
            return _imageContext.Image.Select(x => new ImageModel
            {
                Id = x.Id,
                Url = x.Url,
                IsDeleted = x.IsDeleted,
                ProductId = x.ProductId
            });
        }

        public async Task<ActionResult> Create(ImageModel image)
        {
            if (_token == null)
            {
                return new NotFoundObjectResult("Отсутствует токен для загрузки изображения в облако");
            }

            if (image == null)
            {
                return new NotFoundObjectResult("Отсутствует изображение для добавления");
            }

            var imageHref = await UploadImageToYandexDisk(image.Url);

            if (imageHref == null)
            {
                return new BadRequestObjectResult("Не удалось загрузить изображение в облако");
            }
            
            var now = DateTime.Now;
            var imageEntity = new ImageEntity
            {
                Id = Guid.NewGuid(),
                CreatedDate = now,
                LastSavedDate = now,
                ProductId = image.ProductId,
                Url = imageHref
            };

            await _imageContext.Image.AddAsync(imageEntity);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult> CreateMany(IEnumerable<ImageModel> images)
        {
            if (_token == null)
            {
                return new NotFoundObjectResult("Отсутствует токен для загрузки изображения в облако");
            }

            if (!images.Any())
            {
                return new NotFoundObjectResult("Отсутствуют изображения для добавления");
            }

            var imagesHrefs = new List<string>();
            foreach (var image in images)
            {
                var imageHref = await UploadImageToYandexDisk(image.Url);
                
                if (imageHref == null)
                {
                    return new BadRequestObjectResult("Не удалось загрузить изображение в облако");
                }

                imagesHrefs.Add(imageHref);
            }

            var now = DateTime.Now;
            var productId = images.Select(x => x.ProductId)
                .FirstOrDefault();

            var imagesEntity = imagesHrefs.Select(
                imageUrl => new ImageEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = now,
                    LastSavedDate = now,
                    ProductId = productId,
                    Url = imageUrl
                });

            await _imageContext.Image.AddRangeAsync(imagesEntity);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult> Update(ImageModel image)
        {
            if (image == null)
            {
                return new NotFoundObjectResult("Отсутствует изображение для изменения");
            }

            var imageEntity = await _imageContext.Image
                .Where(x => x.Id == image.Id)
                .FirstOrDefaultAsync();
            
            if (imageEntity == null)
            {
                return new NotFoundObjectResult("Изображение не найдено в БД");
            }

            imageEntity.ProductId = image.ProductId;
            imageEntity.Url = image.Url;
            imageEntity.LastSavedDate = DateTime.Now;
            imageEntity.IsDeleted = image.IsDeleted;

            _imageContext.Image.Update(imageEntity);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult> UpdateMany(IEnumerable<ImageModel> images)
        {
            if (!images.Any())
            {
                return new NotFoundObjectResult("Отсутствуют изображения для изменения");
            }

            var imagesIds = images
                .Select(x => x.Id)
                .ToArray();
            
            var imageEntity = await _imageContext.Image
                .Where(x => imagesIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            if (!imageEntity.Any())
            {
                return new NotFoundObjectResult("Изображения не найдены в БД");
            }

            foreach (var image in images)
            {
                if (imageEntity.TryGetValue(image.Id, out var entity))
                {
                    entity.ProductId = image.ProductId;
                    entity.Url = image.Url;
                    entity.LastSavedDate = DateTime.Now;
                    entity.IsDeleted = image.IsDeleted;
                };
            }

            _imageContext.Image.UpdateRange(imageEntity.Values);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Отсутствует идентификатор изображения");
            }

            var imageEntity = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (imageEntity == null)
            {
                return new NotFoundObjectResult("Изображение не найдено в БД");
            }

            imageEntity.LastSavedDate = DateTime.Now;
            imageEntity.IsDeleted = true;

            _imageContext.Image.Update(imageEntity);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            if (!ids.Any())
            {
                return new NotFoundObjectResult("Отсутствуют идентификаторы изображений");
            }

            var imageEntity = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (!imageEntity.Any())
            {
                return new NotFoundObjectResult("Изображения не найдены в БД");
            }

            foreach (var image in imageEntity)
            {
                image.LastSavedDate = DateTime.Now;
                image.IsDeleted = true;
            }

            _imageContext.Image.UpdateRange(imageEntity);
            await _imageContext.SaveChangesAsync();

            return new NoContentResult();
        }

        private async Task<string> UploadImageToYandexDisk(string imageUrl)
        {
            try
            {
                var fullPath = GetFullPathForImage(imageUrl);
                var response = await _yandexDiskImageClient.UploadImageToYandexDisk(imageUrl, fullPath, _token);
                var deserializedResponse = JsonConvert.DeserializeObject<UploadResponse>(response);

                return deserializedResponse.Href;
            }
            catch (Exception)
            {
                // ToDo: Вот так делать нежелательно
                return null;
            }
        }

        /// <summary>
        /// Сформировать полный путь для заливки изображения в папку CustomImageFolder 
        /// </summary>
        /// <param name="imageUrl">Ссылка на изображение</param>
        /// <returns>Полный путь до изображения на яндекс диске</returns>
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