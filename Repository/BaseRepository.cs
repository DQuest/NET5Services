using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Repository
{
    // ToDo переделать [Price] под обобщение
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        public readonly string _connectionString;
        public readonly string _tableName;
        public readonly IOptions<DbOptions> _dbOptions;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IOptions<DbOptions> dbOptions, IHttpContextAccessor httpContextAccessor, string tableName)
        {
            _connectionString = _dbOptions.Value.ConnectionString;
            _tableName = tableName;
            _dbOptions = dbOptions;

            _httpContextAccessor = httpContextAccessor;
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
            try
            {
                await using var db = await GetSqlConnection();

                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.UtcNow;
                entity.LastSavedDate = DateTime.UtcNow;

                if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId))
                {
                    entity.CreatedBy = userId;
                    entity.LastSavedBy = userId;
                }

                var fields = string.Join(", ", typeof(T).GetProperties().Select(prop => $"[{prop.Name}]"));
                var values = string.Join(", ", typeof(T).GetProperties().Select(prop => $"@{prop.Name}"));

                await db.ExecuteAsync($"INSERT INTO [{_tableName}] ({fields}) VALUES ({values})", entity);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task Update(T entity)
        {
            try
            {
                await using var db = await GetSqlConnection();

                entity.LastSavedDate = DateTime.UtcNow;

                if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId))
                {
                    entity.LastSavedBy = userId;
                }

                var notUpdateFields = new[] {"Id", "CreatedDate", "CreatedBy", "IsDeleted"};
                var parameters = string.Join(", ",
                    typeof(T).GetProperties().Where(prop => !notUpdateFields.Contains(prop.Name))
                        .Select(prop => $"{prop.Name} = @{prop.Name}"));

                await db.ExecuteAsync($"UPDATE [{_tableName}] SET ({parameters}) WHERE [Id] = @Id", entity);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task Delete(Guid id)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE [Price] SET [IsDeleted] = 1 WHERE [Id] = @Id", new {Id = id});
        }

        public virtual async Task CreateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.UtcNow;
                entity.LastSavedDate = DateTime.UtcNow;

                if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId))
                {
                    entity.CreatedBy = userId;
                    entity.LastSavedBy = userId;
                }

                var fields = string.Join(", ", typeof(T).GetProperties().Select(prop => $"[{prop.Name}]"));
                var values = string.Join(", ", typeof(T).GetProperties().Select(prop => $"@{prop.Name}"));

                await db.ExecuteAsync($"INSERT INTO [{_tableName}] ({fields}) VALUES ({values})", entities);
            }
        }

        public virtual async Task UpdateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                entity.LastSavedDate = DateTime.UtcNow;

                if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    out var userId))
                {
                    entity.LastSavedBy = userId;
                }

                var notUpdateFields = new[] {"Id", "CreatedDate", "CreatedBy", "IsDeleted"};
                var parameters = string.Join(", ",
                    typeof(T).GetProperties().Where(prop => !notUpdateFields.Contains(prop.Name))
                        .Select(prop => $"{prop.Name} = @{prop.Name}"));

                await db.ExecuteAsync($"UPDATE [{_tableName}] SET ({parameters}) WHERE [Id] = @Id", entities);
            }
        }

        public virtual async Task DeleteMany(IEnumerable<Guid> ids)
        {
            await using var db = await GetSqlConnection();

            foreach (var id in ids)
            {
                await db.ExecuteAsync($"UPDATE [{_tableName}] SET [IsDeleted] = 1 WHERE [Id] = @Id", new {Id = id});
            }
        }

        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(_connectionString);
            db.Open();
            return db;
        }
    }
}