namespace Homework4.CustomImageService.Models
{
    public class UploadResponseModel
    {
        /// <summary>
        /// URL.
        /// </summary>
        public string Href {get;set;}

        /// <summary>
        /// Признак шаблонизированного URL.
        /// </summary>
        public bool Templated {get;set;}
    }
}