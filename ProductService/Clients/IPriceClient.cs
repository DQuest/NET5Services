using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models;
using Refit;

namespace ProductService.Clients
{
    public interface IPriceClient
    {
        /// <summary>
        /// Получить все цены продукта (даже удалённые).
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        [Get("/api/prices")]
        Task<IEnumerable<PriceModel>> GetAllPricesForProduct(Guid productId);
        
        /// <summary>
        /// Получить актуальную стоимость продукта.
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        [Get("/api/prices/GetActualPriceForProduct/{productId}")]
        Task<PriceModel> GetActualPriceForProduct(Guid productId);
        
        /// <summary>
        /// Установить цену продукту.
        /// </summary>
        /// <param name="price">Модель ценника</param>
        /// <returns></returns>
        [Post("/api/prices")]
        Task SetNewPriceForProduct(PriceModel price);

        /// <summary>
        /// Обновить стоимость.
        /// </summary>
        /// <param name="price">Модель ценника</param>
        /// <returns></returns>
        [Put("/api/prices")]
        Task UpdatePriceForProduct(PriceModel price);

        /// <summary>
        /// Удалить все цены продуктам.
        /// </summary>
        /// <param name="productsIds">Идентификаторы продуктов</param>
        /// <returns></returns>
        [Delete("/api/prices")]
        Task DeletePricesForProducts(IEnumerable<Guid> productsIds);
    }
}