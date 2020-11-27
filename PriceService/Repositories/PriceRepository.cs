using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PriceService.Interfaces;
using BaseRepository;
using Dapper;

namespace PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string TableName => "Price";

        public PriceRepository(IOptions<PriceDbOptions> dbOptions, IHttpContextAccessor httpContextAccessor) 
            : base(dbOptions, TableName, httpContextAccessor)
        {
        }

        // ToDo доработка методов работы с БД для модели Price

        public override async Task<PriceDbModel> Get(Guid productId)
        {
            await using var db = await GetSqlConnection();
            return await db.QueryFirstOrDefaultAsync<PriceDbModel>(
                $"SELECT * FROM {TableName} WHERE ProductId = @ProductId AND IsDeleted = 0", new {ProductId = productId});
        }
    }
}