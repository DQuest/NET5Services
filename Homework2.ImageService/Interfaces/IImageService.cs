using System.Collections.Generic;
using System.Threading.Tasks;
using Homework2.ImageService.Entities;

namespace Homework2.ImageService.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<ImageEntity>> GetAll();
    }
}