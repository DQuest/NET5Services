namespace Homework2.Services.ImageService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/images")]
    public class ImageController : Controller, IImageController
    {
        private IEnumerable<ImageModel> _images;

        public ImageController()
        {
            FillImages();
        }

        [HttpGet]
        public IEnumerable<ImageModel> GetAll()
        {
            return _images;
        }

        [HttpGet("{id}")]
        public ImageModel Get(long id)
        {
            var image = _images.FirstOrDefault(x => x.Id == id);

            if (image == null)
            {
                throw new ArgumentException("Нет такого изображения");
            }

            return image;
        }

        private void FillImages()
        {
            _images = new List<ImageModel>
            {
                new ImageModel {Id = 1, ImageName = "FirstImage", ImagePath = "FirstPath"},
                new ImageModel {Id = 2, ImageName = "SecondImage", ImagePath = "SecondPath"},
                new ImageModel {Id = 3, ImageName = "ThirdImage", ImagePath = "ThirdPath"},
                new ImageModel {Id = 4, ImageName = "FourthImage", ImagePath = "FourthPath"},
                new ImageModel {Id = 5, ImageName = "FifthImage", ImagePath = "FifthPath"}
            };
        }
    }
}