using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(Guid id);

        Task<IEnumerable<T>> GetAll();

        Task Create(T entity);

        Task CreateMany(IEnumerable<T> entities);

        Task Update(T entity);

        Task UpdateMany(IEnumerable<T> entities);

        Task Delete(Guid id);

        Task DeleteMany(IEnumerable<Guid> id);

        // Task<bool> Restore(Guid id);
        //
        // Task<bool> RestoreMany(IEnumerable<Guid> id);
    }
}