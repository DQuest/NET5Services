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
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        public readonly string _connectionString;
        public readonly string _tableName;
        public readonly IOptions<DbOptions> _dbOptions;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IOptions<DbOptions> dbOptions, string tableName, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = dbOptions.Value.ConnectionString;
            _tableName = tableName;
            _dbOptions = dbOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            await using var db = await GetSqlConnection();
            return await db.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE IsDeleted = 0");
        }

        public virtual async Task<T> Get(Guid id)
        {
            await using var db = await GetSqlConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM {_tableName} WHERE Id = @Id AND IsDeleted = 0", new {Id = id});
        }

        public virtual async Task Create(T entity)
        {
            try
            {
                await using var db = await GetSqlConnection();

                FillBaseFields(entity);
                var (fields, values) = FillDbStructureForCreate();

                await db.ExecuteAsync($"INSERT INTO {_tableName} ({fields}) VALUES ({values})", entity);
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

                FillBaseFields(entity, true);

                var parameters = FillDbStructureForUpdate();

                await db.ExecuteAsync($"UPDATE {_tableName} SET {parameters} WHERE [Id] = @Id", entity);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task Delete(Guid id)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE {_tableName} SET IsDeleted = true WHERE [Id] = @Id", new {Id = id});
        }

        public virtual async Task CreateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                FillBaseFields(entity);

                var (fields, values) = FillDbStructureForCreate();

                await db.ExecuteAsync($"INSERT INTO {_tableName} ({fields}) VALUES ({values})", entities);
            }
        }

        public virtual async Task UpdateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                FillBaseFields(entity, true);

                var parameters = FillDbStructureForUpdate();

                await db.ExecuteAsync($"UPDATE {_tableName} SET {parameters} WHERE [Id] = @Id", entities);
            }
        }

        public virtual async Task DeleteMany(IEnumerable<Guid> ids)
        {
            await using var db = await GetSqlConnection();

            foreach (var id in ids)
            {
                await db.ExecuteAsync($"UPDATE {_tableName} SET IsDeleted = 1 WHERE [Id] = @Id", new {Id = id});
            }
        }

        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(_connectionString);
            db.Open();
            return db;
        }
        
        private (string fields, string values) FillDbStructureForCreate()
        {
            var fields = string.Join(", ", typeof(T).GetProperties().Select(prop => $"[{prop.Name}]"));
            var values = string.Join(", ", typeof(T).GetProperties().Select(prop => $"@{prop.Name}"));
            return (fields, values);
        }

        private static string FillDbStructureForUpdate()
        {
            var notUpdateFields = new[] {"Id", "CreatedDate", "CreatedBy", "IsDeleted"};
            return string.Join(", ",
                typeof(T).GetProperties().Where(prop => !notUpdateFields.Contains(prop.Name))
                    .Select(prop => $"{prop.Name} = @{prop.Name}"));
        }

        private void FillBaseFields(T entity, bool isUpdateOperation = false)
        {
            if (!isUpdateOperation)
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.UtcNow;
            }

            entity.LastSavedDate = DateTime.UtcNow;

            if (Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                if (!isUpdateOperation)
                {
                    entity.CreatedBy = userId;
                }

                entity.LastSavedBy = userId;
            }
            
        }
    }
}