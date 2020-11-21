using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PriceService.Interfaces;
using Repository;

namespace PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string _tableName => "Price";

        public PriceRepository(IOptions<PriceDbOptions> dbOptions) 
            : base(dbOptions, _tableName)
        {
        }

        // ToDo доработка методов работы с БД для модели Price
    }
}