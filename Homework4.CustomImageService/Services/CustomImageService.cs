using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Homework4.CustomImageService.Clients;
using Homework4.CustomImageService.Interfaces;
using Homework4.CustomImageService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Homework4.CustomImageService.Services
{
    /// <summary>
    /// Сервис для работы с файлами на Яндекс Диске
    /// </summary>
    public class CustomImageService : ICustomImageService
    {
        private readonly string _token;
        private readonly IYandexDriveImageClient _yandexDriveImageClient;
        private readonly IImageDbClient _imageDbClient;

        public CustomImageService(
            IConfiguration cfg,
            IYandexDriveImageClient yandexDriveImageClient, 
            IImageDbClient imageDbClient)
        {
            _token = cfg.GetValue<string>("YandexToken");
            _yandexDriveImageClient = yandexDriveImageClient ?? throw new ArgumentException(nameof(yandexDriveImageClient));
            _imageDbClient = imageDbClient ?? throw new ArgumentException(nameof(yandexDriveImageClient));
        }

        /// <summary>
        /// Получить список изображений напрямую с яндекс диска.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            try
            {
                // Like JsonConvert.DeserializeObject<CustomImageModel> from string
                var data = await _yandexDriveImageClient.GetAll(_token);
                return data.Images;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Получить ссылку на изображение в яндекс диске из БД по Id записи.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> GetImageUrl(Guid id)
        {
            try
            {
                var image = await _imageDbClient.Get(id);

                if (image == null)
                {
                    throw new ArgumentException($"Изображение с идентификатором {id} не найдено в БД.");
                }

                return image.Url;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        
        /// <summary>
        /// Получить метаинфу изображения с Яндекс Диска с помощью Id записи в БД.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ImageModel> GetImageMetaInfo(Guid id)
        {
            try
            {
                var imageInfoFromDb = await _imageDbClient.Get(id);
                var imageFromYandex = await _yandexDriveImageClient.Get(imageInfoFromDb.FullPathOnDisk,_token);

                if (imageFromYandex == null)
                {
                    throw new ArgumentException($"По ссылке {imageInfoFromDb.FullPathOnDisk} не найдено изображений в Яндекс Диске.");
                }

                return imageFromYandex;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Загрузить файл в Диск по URL.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task Upload(string imageUrl)
        {
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