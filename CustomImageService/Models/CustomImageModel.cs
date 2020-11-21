using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomImageService.Models
{
    public class CustomImageModel
    {
        [JsonProperty("items")]
        public IEnumerable<ImageModel> Images { get; set; } 
    }
}