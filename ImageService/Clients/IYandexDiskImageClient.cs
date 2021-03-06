using System.Threading.Tasks;
using ImageService.Models;
using Refit;

namespace ImageService.Clients
{
    public interface IYandexDiskImageClient
    {
        /// <summary>
        /// Получить список изображений с яндекс диска.
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [Get("/v1/disk/resources/files?media_type=image")]
        Task<ImageModel> GetAllImagesFromYandexDisk([Header("Authorization")] string authorization);
        
        /// <summary>
        /// Получить изображение с яндекс диска по указанному полному пути.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [Get("/v1/disk/resources?path={fullPath}")]
        Task<ImageModel> GetImageFromYandexDisk(string fullPath, [Header("Authorization")] string authorization);

        /// <summary>
        /// Загрузить файл в Диск по URL.
        /// </summary>
        /// <returns></returns>
        [Post("/v1/disk/resources/upload?path={fullPath}&url={imageUrl}&fields=href")]
        Task<string> UploadImageToYandexDisk(string imageUrl, string fullPath, [Header("Authorization")] string authorization);
    }
}