using System;
using System.Collections.Generic;

namespace ImageService.Models
{
    public class UploadImagesModel
    {
        public Guid ProductId { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}