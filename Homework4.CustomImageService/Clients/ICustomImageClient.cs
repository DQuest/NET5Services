using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.CustomImageService.Models;
using Refit;

namespace Homework4.CustomImageService.Clients
{
    public interface ICustomImageClient
    {
        [Get("/v1/disk/resources/files?media_type=image")]
        Task<string> GetAll([Header("Authorization")] string authorization = "OAuth AgAAAAAOsvxOAADLW9hwcbHZWEm3m8OA9ZIIlwg");
        
        [Post("/v1/disk/resources/upload")]
        Task<CustomImageModel> Post();
    }
}