using System;

namespace ProductService.Models
{
    public class ImageModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Url { get; set; }
    }
}