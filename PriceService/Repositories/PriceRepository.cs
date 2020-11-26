using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PriceService.Interfaces;
using BaseRepository;

namespace PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string _tableName => "Price";

        public PriceRepository(IOptions<PriceDbOptions> dbOptions, IHttpContextAccessor httpContextAccessor) 
            : base(dbOptions, _tableName, httpContextAccessor)
        {
        }

        // ToDo доработка методов работы с БД для модели Price
    }
}