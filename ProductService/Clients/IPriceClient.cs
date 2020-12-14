using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using Refit;

namespace ProductService.Clients
{
    public interface IPriceClient
    {
        /// <summary>
        /// Получение цены для продукта.
        /// </summary>
        /// <returns></returns>
        [Get("/ProductPrice/{productId}")]
        Task<PriceModel> GetPriceForProduct(Guid productId);
        
        /// <summary>
        /// Получение определённой цены.
        /// </summary>
        /// <param name="id">Id цены</param>
        /// <returns></returns>
        [Get("/Price/Get/{id}")]
        Task<PriceModel> Get(Guid id);

        /// <summary>
        /// Добавить стоимость.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Post("/Price/Create")]
        Task<ActionResult> Create(PriceModel price);

        /// <summary>
        /// Добавить цены.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Post("/Price/CreateMany")]
        Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Put("/Price/Update")]
        Task<ActionResult> Update(PriceModel price);
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Put("/Price/UpdateMany")]
        Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Удалить стоимость.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Delete("/Price/Delete")]
        Task<ActionResult> Delete(Guid id);

        /// <summary>
        /// Удалить цены.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Delete("/Price/DeleteMany")]
        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}