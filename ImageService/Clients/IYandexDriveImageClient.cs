using System.Threading.Tasks;
using ImageService.Models;
using Refit;

namespace ImageService.Clients
{
    public interface IYandexDriveImageClient
    {
        /// <summary>
        /// Получить список изображений.
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [Get("/v1/disk/resources/files?media_type=image")]
        Task<ImageYandexModel> GetAll([Header("Authorization")] string authorization);
        
        /// <summary>
        /// Получить изображение с яндекс диска по указанному полному пути.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [Get("/v1/disk/resources?path={fullPath}")]
        Task<ImageYandexModel> Get(string fullPath, [Header("Authorization")] string authorization);

        /// <summary>
        /// Загрузить файл в Диск по URL.
        /// </summary>
        /// <returns></returns>
        [Post("/v1/disk/resources/upload?path={fullPath}&url={imageUrl}")]
        Task<string> Upload(string imageUrl, string fullPath, [Header("Authorization")] string authorization);
    }
}