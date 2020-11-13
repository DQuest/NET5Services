using Newtonsoft.Json;

namespace Homework4.CustomImageService.Models
{
    public class ImageModel
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("preview")]
        public string Preview { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}