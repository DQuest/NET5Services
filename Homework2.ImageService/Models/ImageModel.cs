namespace Homework2.ImageService.Models
{
    public class ImageModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование изображения.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Путь до изображения в файловом хранилище.
        /// </summary>
        public string ImagePath { get; set; }
    }
}