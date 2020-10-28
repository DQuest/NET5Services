using System;
using System.Collections.Generic;
using System.Linq;
using Homework2.ImageService.Interfaces;
using Homework2.ImageService.Models;

namespace Homework2.ImageService.Services
{
    public class ImageService : IImageService
    {
        private IEnumerable<ImageModel> _images;

        public ImageService()
        {
            FillImages();
        }

        public IEnumerable<ImageModel> GetAll()
        {
            return _images;
        }

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