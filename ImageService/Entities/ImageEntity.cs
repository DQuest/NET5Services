using System;
using BaseRepository;

namespace ImageService.Entities
{
    public class ImageEntity : BaseEntity
    {
        public Guid ProductId { get; set; }

        public string Url { get; set; }
    }
}