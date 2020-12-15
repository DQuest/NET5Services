using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using PriceService.Interfaces;
using BaseRepository;
using Microsoft.AspNetCore.Mvc;
using PriceService.Models;

namespace PriceService.Repositories
{
    public class ProductPriceRepository : BaseRepository<PriceDbModel>, IProductPriceRepository
    {
        private static string TableName = "Price";

        private readonly IMapper _mapper;

        public ProductPriceRepository(IOptions<PriceDbOptions> dbOptions, IMapper mapper)
            : base(dbOptions, TableName)
        {
            _mapper = mapper;
        }

        public async Task<ActionResult<PriceModel>> GetPriceForProduct(Guid productId)
        {
            var allPrices = await base.GetAll();

            var priceForProduct = allPrices
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ProductId == productId)
                .FirstOrDefault(x => x.IsLast);

            if (priceForProduct == null)
            {
                return new NotFoundObjectResult($"Цена для продукта с идентификатором {productId} не найдена в БД");
            }

            // gives emphasis of the content that is returned
            return new ObjectResult(_mapper.Map<PriceModel>(priceForProduct));
        }

        public async Task<ActionResult> DeletePriceForProduct(Guid productId)
        {
            var allPrices = await base.GetAll();

            var priceForProduct = allPrices
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ProductId == productId)
                .FirstOrDefault(x => x.IsLast);

            if (priceForProduct == null)
            {
                return new NotFoundObjectResult($"Цена для продукта с идентификатором {productId} не найдена в БД");
            }

            priceForProduct.IsDeleted = true;
            priceForProduct.LastSavedDate = DateTime.Now;
            priceForProduct.IsLast = false;

            await base.Update(priceForProduct);

            return new NoContentResult();
        }
    }
}