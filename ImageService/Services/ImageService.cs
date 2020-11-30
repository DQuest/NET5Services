using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ImageService(
            ImageContext imageContext, 
            IYandexDiskImageClient yandexDiskImageClient, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration cfg,
            IMapper mapper)
        {
            _token = cfg.GetValue<string>("YandexToken");
            _imageContext = imageContext ?? throw new ArgumentException(nameof(imageContext));
            _yandexDiskImageClient = yandexDiskImageClient ?? throw new ArgumentException(nameof(yandexDiskImageClient));;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все изображения для определённого продукта.
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns>Список изображений</returns>
        public async Task<IEnumerable<ImageModel>> GetAllImagesForProduct(Guid productId)
        {
            var imageEntity = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ImageModel>>(imageEntity);
        }

        public async Task<ImageModel> GetImage(Guid imageId)
        {
            var imageEntity = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == imageId);

            if (imageEntity != null)
            {
                return _mapper.Map<ImageModel>(imageEntity);
            }

            return new ImageModel();
        }

        /// <summary>
        /// Залить изображения для продукта и записать ссылки в БД.
        /// </summary>
        /// <param name="uploadImagesModel">Модель для заливки изображений для продукта</param>
        /// <returns></returns>
        public async Task UploadImagesForProduct(UploadImagesModel uploadImagesModel)
        {
            var dateTimeNow = DateTime.Now;

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

        /// <summary>
        /// Обновление информации об изображении в БД.
        /// </summary>
        /// <param name="image">Модель изображения</param>
        /// <returns></returns>
        public async Task UpdateImage(ImageModel image)
        {
            try
            {
                var imageEntity = await _imageContext.Image
                    .Where(x => x.IsDeleted == false)
                    .FirstOrDefaultAsync(x => x.Id == image.Id);

                imageEntity.Url = image.Url;
                imageEntity.ProductId = image.ProductId;
                imageEntity.LastSavedDate = DateTime.Now;

                if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId))
                {
                    imageEntity.LastSavedBy = userId;
                }

                _imageContext.Image.Update(imageEntity);
                await _imageContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Удалить все изображения связанные с продкутом.
        /// </summary>
        /// <param name="productsIds">Id продукта</param>
        /// <returns></returns>
        public async Task DeleteImagesForProducts(IEnumerable<Guid> productsIds)
        {
            var productsImages = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .Where(x => productsIds.Contains(x.ProductId))
                .ToListAsync();

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId);

            var dateTimeNow = DateTime.Now;
            foreach (var image in productsImages)
            {
                image.LastSavedBy = userId;
                image.LastSavedDate = dateTimeNow;
                image.IsDeleted = true;
            }

            _imageContext.Image.UpdateRange(productsImages);
            await _imageContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Заливка изображений на яндекс диск.
        /// </summary>
        /// <param name="imagesUrls">Ссылки на изображения со сторонних ресурсов</param>
        /// <returns>Ссылки на изображения на яндекс диске</returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<List<string>> UploadImagesToYandexDisk(IEnumerable<string> imagesUrls)
        {
            var imagesHrefs = new List<string>();
            try
            {
                foreach (var imageUrl in imagesUrls)
                {
                    var fullPath = GetFullPathForImage(imageUrl);
                    var response = await _yandexDiskImageClient.UploadImageToYandexDisk(imageUrl, fullPath, _token);
                    var deserializedResponse = JsonConvert.DeserializeObject<UploadResponse>(response);

                    imagesHrefs.Add(deserializedResponse.Href);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return imagesHrefs;
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