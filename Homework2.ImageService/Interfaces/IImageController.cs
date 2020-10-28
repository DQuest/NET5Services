namespace Homework2.ImageService.Interfaces
{
    using System.Collections.Generic;
    using Homework2.ImageService.Models;

    public interface IImageController
    {
        IEnumerable<ImageModel> GetAll();

        ImageModel Get(long id);
    }
}