using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.CustomImageService.Models;

namespace Homework4.CustomImageService.Interfaces
{
    public interface ICustomImageService
    {
        Task<IEnumerable<ImageModel>> GetAll();

        Task Post();
    }
}