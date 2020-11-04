using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T: BaseEntity
    {
        public readonly IOptions<DbOptions> _dbOptions;
        public readonly string _connectionString;

        public BaseRepository(IOptions<DbOptions> dbOptions)
        {
            _dbOptions = dbOptions;
            _connectionString = _dbOptions.Value.ConnectionString;
        }
        
        public virtual async Task<T> GetById(Guid id)
        {
            await using var db = await GetSqlConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM [{typeof(T).Name}] WHERE [Id] = @Id AND [IsDeleted] = 0", new { id });
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            await using var db = await GetSqlConnection();
            return await db.QueryAsync<T>($"SELECT * FROM [{typeof(T).Name}] WHERE [IsDeleted] = 0");
        }

        public Task<T> Create(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> CreateMany(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMany(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMany(IEnumerable<Guid> id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Restore(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestoreMany(IEnumerable<Guid> id)
        {
            throw new NotImplementedException();
        }

        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(_connectionString);
            db.Open();
            return db;
        }
    }
}