using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriceService.Models;

namespace PriceService.Interfaces
{
    public interface IPriceRepository
    {
        Task<IEnumerable<PriceModel>> GetAllPricesForProduct(Guid productId);
        
        Task<PriceModel> GetActualPriceForProduct(Guid productId);

        Task SetNewPriceForProduct(PriceModel price);

        Task UpdatePriceForProduct(PriceModel price);

        Task DeletePricesForProducts(IEnumerable<Guid> productsIds);
    }
}