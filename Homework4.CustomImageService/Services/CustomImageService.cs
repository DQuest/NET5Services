using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Homework4.CustomImageService.Clients;
using Homework4.CustomImageService.Interfaces;
using Homework4.CustomImageService.Models;
using Microsoft.Extensions.Configuration;

namespace Homework4.CustomImageService.Services
{
    /// <summary>
    /// Сервис для работы с файлами на Яндекс Диске
    /// </summary>
    public class CustomImageService : ICustomImageService
    {
        private readonly string _token;

        private readonly ICustomImageClient _customImageClient;

        public CustomImageService(ICustomImageClient customImageClient, IConfiguration cfg)
        {
            _token = cfg.GetValue<string>("YandexToken");
            _customImageClient = customImageClient ?? throw new ArgumentException(nameof(customImageClient));
        }

        /// <summary>
        /// Получить список изображений.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<ImageModel>> GetAll()
        {
            try
            {
                // Like JsonConvert.DeserializeObject<CustomImageModel> from string
                var data = await _customImageClient.GetAll(_token);
                return data.Images;
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
                // todo сделать разбивку
                var regex = new Regex(@"/[\w-]+\.(jpg|png|jpeg|bmp|gif)/g");
                var imageName = regex.Split(imageUrl);
                var fullPath = $"CustomImageFolder/{imageName}";
                await _customImageClient.Upload(imageUrl, fullPath, _token);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}