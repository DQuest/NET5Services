namespace Homework2.Models
{
    using System.Collections.Generic;
    using Homework2.ImageService.Models;
    using Homework2.PriceService.Models;

    public class ProductModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Изображения товара.
        /// </summary>
        public IEnumerable<ImageModel> Images { get; set; }

        /// <summary>
        /// Цены на товар.
        /// </summary>
        public IEnumerable<PriceModel> Prices { get; set; }
    }
}