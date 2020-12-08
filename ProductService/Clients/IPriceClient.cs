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
        /// Получение цен.
        /// </summary>
        /// <returns></returns>
        [Get("/price/GetAll")]
        IQueryable<PriceModel> GetAll();
        
        /// <summary>
        /// Получение определённой цены.
        /// </summary>
        /// <param name="id">Id цены</param>
        /// <returns></returns>
        [Get("/price/Get/{id}")]
        Task<ActionResult<PriceModel>> Get(Guid id);

        /// <summary>
        /// Добавить стоимость.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Post("/price/Create")]
        Task<ActionResult> Create(PriceModel price);

        /// <summary>
        /// Добавить цены.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Post("/price/CreateMany")]
        Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Put("/price/Update")]
        Task<ActionResult> Update(PriceModel price);
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Put("/price/UpdateMany")]
        Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Удалить стоимость.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Delete("/price/Delete")]
        Task<ActionResult> Delete(Guid id);

        /// <summary>
        /// Удалить цены.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Delete("/price/DeleteMany")]
        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}