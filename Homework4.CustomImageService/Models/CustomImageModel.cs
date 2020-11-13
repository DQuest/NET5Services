using System.Collections.Generic;
using Newtonsoft.Json;

namespace Homework4.CustomImageService.Models
{
    public class CustomImageModel
    {
        [JsonProperty("items")]
        public IEnumerable<ImageModel> Images { get; set; } 
    }
}