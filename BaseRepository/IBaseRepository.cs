using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseRepository
{
    public interface IBaseRepository<T>
    {
        Task<T> Get(Guid id);

        Task<IEnumerable<T>> GetAll();

        Task Create(T entity);

        Task CreateMany(IEnumerable<T> entities);

        Task Update(T entity);

        Task UpdateMany(IEnumerable<T> entities);

        Task Delete(Guid id);

        Task DeleteMany(IEnumerable<Guid> id);
    }
}