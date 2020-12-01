using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PriceService.Interfaces;
using BaseRepository;
using Dapper;
using PriceService.Models;

namespace PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string TableName => "Price";
        
        private readonly IMapper _mapper;

        public PriceRepository(
            IOptions<PriceDbOptions> dbOptions, 
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper) 
            : base(dbOptions, TableName, httpContextAccessor)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public async Task<IEnumerable<PriceModel>> GetAllPricesForProduct(Guid productId)
        {
            await using var db = await GetSqlConnection();
            var priceEntity = await db.QueryAsync<PriceDbModel>(
                $"SELECT * " +
                $"FROM {TableName} " +
                $"WHERE ProductId = @ProductId", new {ProductId = productId});

            return _mapper.Map<IEnumerable<PriceModel>>(priceEntity);
        }

        public async Task<PriceModel> GetActualPriceForProduct(Guid productId)
        {
            await using var db = await GetSqlConnection();
            var priceEntity = await db.QueryFirstOrDefaultAsync<PriceDbModel>(
                $"SELECT * " +
                $"FROM {TableName} " +
                $"WHERE ProductId = @ProductId " +
                $"AND IsDeleted = 0 " +
                $"AND IsLast = 1", new {ProductId = productId});

            return _mapper.Map<PriceModel>(priceEntity);
        }

        public async Task SetNewPriceForProduct(PriceModel price)
        {
            // Перед установкой новых цен для продукта, старые удаляем.
            await DeletePricesForProducts(new[] {price.ProductId});
            
            var priceEntity = _mapper.Map<PriceDbModel>(price);
            priceEntity.IsLast = true;
            
            await base.Create(priceEntity);
        }

        public async Task UpdatePriceForProduct(PriceModel price)
        {
            var priceEntity = _mapper.Map<PriceDbModel>(price);
            await base.Update(priceEntity);
        }

        public async Task DeletePricesForProducts(IEnumerable<Guid> productsIds)
        {
            await using var db = await GetSqlConnection();
            await db.ExecuteAsync($"UPDATE {TableName} " +
                                  $"SET IsDeleted = 1, IsLast = 0 " +
                                  $"WHERE IsDeleted = 0 " +
                                  $"AND IsLast = 1 " +
                                  $"AND ProductId IN @productsIds", new {productsIds});
        }
    }
}