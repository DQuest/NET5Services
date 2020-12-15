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
        /// Получение цены для продукта.
        /// </summary>
        /// <returns></returns>
        [Delete("/ProductPrice/{productId}")]
        Task<ApiResponse<PriceModel>> DeletePriceForProduct(Guid productId);
        
        /// <summary>
        /// Получение определённой цены.
        /// </summary>
        /// <param name="id">Id цены</param>
        /// <returns></returns>
        [Get("/Price/{id}")]
        Task<ApiResponse<PriceModel>> Get(Guid id);

        /// <summary>
        /// Добавить стоимость.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Post("/Price")]
        Task<ApiResponse<PriceModel>> Create(PriceModel price);

        /// <summary>
        /// Добавить цены.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Post("/Price/CreateMany")]
        Task<ApiResponse<PriceModel>> CreateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Обновить информацию об изображении.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [Put("/Price")]
        Task<ApiResponse<PriceModel>> Update(PriceModel price);
        
        /// <summary>
        /// Обновить информацию об изображениях.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        [Put("/Price/UpdateMany")]
        Task<ApiResponse<PriceModel>> UpdateMany(IEnumerable<PriceModel> prices);

        /// <summary>
        /// Удалить стоимость.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Delete("/Price")]
        Task<ApiResponse<PriceModel>> Delete(Guid id);

        /// <summary>
        /// Удалить цены.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Delete("/Price/DeleteMany")]
        Task<ApiResponse<PriceModel>> DeleteMany(IEnumerable<Guid> ids);
    }
}