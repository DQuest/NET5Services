using Microsoft.Extensions.Options;
using Repository;

namespace Homework2.PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>
    {
        public PriceRepository(IOptions<PriceDbOptions> dbOptions) : base(dbOptions)
        {
        }
    }
}