using System;

namespace CustomImageService.Models
{
    public class ImageDbModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Url { get; set; }

        public string FullPathOnDisk { get; set; }
    }
}