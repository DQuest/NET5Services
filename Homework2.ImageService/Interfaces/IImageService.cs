using System.Collections.Generic;
using Homework2.ImageService.Models;

namespace Homework2.ImageService.Interfaces
{
    public interface IImageService
    {
        IEnumerable<ImageModel> GetAll();

        ImageModel Get(long id);
    }
}