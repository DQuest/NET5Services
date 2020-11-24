using System;
using Newtonsoft.Json;

namespace ImageService.Entities
{
    public class ImageEntity
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        [JsonProperty("preview")]
        public string PreviewUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime LastSavedDate { get; set; }

        public Guid LastSavedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}