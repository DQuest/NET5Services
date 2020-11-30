using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        public readonly string ConnectionString;
        public readonly string TableName;
        public readonly IOptions<DbOptions> DbOptions;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(IOptions<DbOptions> dbOptions, string tableName, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = dbOptions.Value.ConnectionString;
            TableName = tableName;
            DbOptions = dbOptions;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            await using var db = await GetSqlConnection();
            return await db.QueryAsync<T>($"SELECT * FROM {TableName} WHERE IsDeleted = 0");
        }

        public virtual async Task<T> Get(Guid id)
        {
            await using var db = await GetSqlConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM {TableName} WHERE Id = @Id AND IsDeleted = 0", new {Id = id});
        }

        public virtual async Task Create(T entity)
        {
            try
            {
                await using var db = await GetSqlConnection();

                FillBaseFields(entity);

                var (fields, values) = FillDbTableStructureForCreate();

                await db.ExecuteAsync($"INSERT INTO {TableName} ({fields}) VALUES ({values})", entity);
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

                var parameters = FillDbTableStructureForUpdate();

                await db.ExecuteAsync($"UPDATE {TableName} SET {parameters} WHERE [Id] = @Id", entity);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task Delete(Guid id)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @Id", new {Id = id});
        }

        public virtual async Task CreateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                FillBaseFields(entity);

                var (fields, values) = FillDbTableStructureForCreate();

                await db.ExecuteAsync($"INSERT INTO {TableName} ({fields}) VALUES ({values})", entities);
            }
        }

        public virtual async Task UpdateMany(IEnumerable<T> entities)
        {
            await using var db = await GetSqlConnection();

            foreach (var entity in entities)
            {
                FillBaseFields(entity, true);

                var parameters = FillDbTableStructureForUpdate();

                await db.ExecuteAsync($"UPDATE {TableName} SET {parameters} WHERE Id = @Id", entities);
            }
        }

        public virtual async Task DeleteMany(IEnumerable<Guid> ids)
        {
            await using var db = await GetSqlConnection();

            foreach (var id in ids)
            {
                await db.ExecuteAsync($"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @Id", new {Id = id});
            }
        }

        protected (string fields, string values) FillDbTableStructureForCreate()
        {
            var fields = string.Join(", ", typeof(T).GetProperties().Select(prop => $"[{prop.Name}]"));
            var values = string.Join(", ", typeof(T).GetProperties().Select(prop => $"@{prop.Name}"));
            return (fields, values);
        }

        protected static string FillDbTableStructureForUpdate()
        {
            var notUpdateFields = new[] {"Id", "CreatedDate", "CreatedBy", "IsDeleted"};
            return string.Join(", ",
                typeof(T).GetProperties().Where(prop => !notUpdateFields.Contains(prop.Name))
                    .Select(prop => $"{prop.Name} = @{prop.Name}"));
        }

        protected void FillBaseFields(T entity, bool isUpdateOperation = false)
        {
            if (!isUpdateOperation)
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.Now;
            }

            entity.LastSavedDate = DateTime.Now;

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
        
        protected async Task<SqlConnection> GetSqlConnection()
        {
            var db = new SqlConnection(ConnectionString);
            db.Open();
            return db;
        }
    }
}