namespace Homework2.Services.ImageService
{
    using System.Collections.Generic;

    public interface IImageController
    {
        IEnumerable<ImageModel> GetAll();

        ImageModel Get(long id);
    }
}