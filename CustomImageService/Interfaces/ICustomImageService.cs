using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomImageService.Models;

namespace CustomImageService.Interfaces
{
    public interface ICustomImageService
    {
        /// <summary>
        /// Получить список изображений.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ImageModel>> GetAll();

        /// <summary>
        /// Получить ссылку на изображение в яндекс диске из БД по Id записи.
        /// </summary>
        /// <returns></returns>
        Task<string> GetImageUrl(Guid id);

        /// <summary>
        /// Получить метаинфу изображения с Яндекс Диска с помощью Id записи в БД.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ImageModel> GetImageMetaInfo(Guid id);

        /// <summary>
        /// Загрузить файл в Диск по URL.
        /// </summary>
        /// <returns></returns>
        Task Upload(string imageUrl);
    }
}