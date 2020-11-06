using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace Repository
{
    // ToDo переделать [Price] под обобщение
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

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            await using var db = await GetSqlConnection();
            return await db.QueryAsync<T>($"SELECT * FROM [Price] WHERE [IsDeleted] = 0");
        }

        public virtual async Task<T> GetById(Guid id)
        {
            await using var db = await GetSqlConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM [Price] WHERE [Id] = @Id AND [IsDeleted] = 0", new {Id = id});
        }

        public virtual async Task Create(T entity)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"INSERT INTO [Price]", new {entity});
        }

        public virtual async Task Update(T entity)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE [Price] SET", entity);
        }
        public virtual async Task Delete(Guid id)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE [Price] SET [IsDeleted] = 1 WHERE [Id] = @Id", new {Id = id});
        }

        #region подумать над "оптимизацией"

        public virtual async Task CreateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();
            
            // todo переделать на транзакцию ?
            // некрасиво спамить запросами БД...
            foreach (var entity in entities)
            {
                await db.ExecuteAsync($"INSERT INTO [Price]", entity);
            }
        }

        public virtual async Task UpdateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();
            
            // todo переделать на транзакцию ?
            // некрасиво спамить запросами БД...
            foreach (var entity in entities)
            {
                await db.ExecuteAsync($"UPDATE [Price] SET", entity);
            }
        }

        public virtual async Task DeleteMany(IEnumerable<Guid> ids)
        {
            await using var db = await GetSqlConnection();

            // todo переделать на транзакцию ?
            // некрасиво спамить запросами БД...
            foreach (var id in ids)
            {
                await db.ExecuteAsync($"UPDATE [Price] SET [IsDeleted] = 1 WHERE [Id] = @Id", new {Id = id});
            }
        }

        #endregion

        #region шта ?

        // public virtual async Task<bool> Restore(Guid id)
        // {
        //     await using var db = await GetSqlConnection();
        // }
        //
        // public virtual async Task<bool> RestoreMany(IEnumerable<Guid> id)
        // {
        //     await using var db = await GetSqlConnection();
        // }

        #endregion

        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(_connectionString);
            db.Open();
            return db;
        }
    }
}