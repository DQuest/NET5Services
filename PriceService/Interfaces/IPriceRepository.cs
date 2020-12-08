using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceService.Models;

namespace PriceService.Interfaces
{
    public interface IPriceRepository
    {
        Task<IEnumerable<PriceModel>> GetAll();
        
        Task<ActionResult<PriceModel>> Get(Guid id);
        
        Task<ActionResult> Create(PriceModel price);

        Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices);

        Task<ActionResult> Update(PriceModel price);
        
        Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices);

        Task<ActionResult> Delete(Guid id);

        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}