using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriceService.Repositories;

namespace PriceService.Interfaces
{
    public interface IPriceRepository
    {
        Task<IEnumerable<PriceDbModel>> GetAll();

        Task<PriceDbModel> Get(Guid id);

        Task Create(PriceDbModel entity);

        Task Update(PriceDbModel entity);

        Task Delete(Guid id);
    }
}