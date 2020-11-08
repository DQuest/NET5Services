using System;

namespace Homework2.ImageService.Models
{
    public class ImageModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Url { get; set; }
    }
}