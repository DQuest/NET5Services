using System;
using System.Collections.Generic;

namespace ProductService.Models.Images
{
    public class UploadImagesModel
    {
        public Guid ProductId { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}