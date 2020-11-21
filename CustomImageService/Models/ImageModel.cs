using Newtonsoft.Json;

namespace CustomImageService.Models
{
    public class ImageModel
    {
        public int Size { get; set; }

        public string Name { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        public string File { get; set; }

        public string Preview { get; set; }

        public string Path { get; set; }
    }
}