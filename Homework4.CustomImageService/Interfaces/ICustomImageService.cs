using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.CustomImageService.Models;

namespace Homework4.CustomImageService.Interfaces
{
    public interface ICustomImageService
    {
        /// <summary>
        /// Получить список изображений.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ImageModel>> GetAll();

        /// <summary>
        /// Загрузить файл в Диск по URL.
        /// </summary>
        /// <returns></returns>
        Task Upload(string imageUrl);
    }
}