using System;
using System.Collections.Generic;
using ProductService.Models.Images;

namespace ProductService.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание продукта.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Изображения товара.
        /// </summary>
        public IEnumerable<ImageModel> Images { get; set; }

        /// <summary>
        /// Цены на товар.
        /// </summary>
        public PriceModel Price { get; set; }
    }
}