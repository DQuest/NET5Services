using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Repository;

namespace Homework2.PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>
    {
        private static string _tableName => "Price";

        public PriceRepository(IOptions<PriceDbOptions> dbOptions, IHttpContextAccessor httpContextAccessor) 
            : base(dbOptions, httpContextAccessor, _tableName)
        {
        }
    }
}