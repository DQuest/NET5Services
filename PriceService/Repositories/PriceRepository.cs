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
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc;
using PriceService.Models;

namespace PriceService.Repositories
{
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string TableName = "Price";

        private readonly IMapper _mapper;

        public PriceRepository(IOptions<PriceDbOptions> dbOptions, IMapper mapper)
            : base(dbOptions, TableName)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public async Task<ActionResult<IEnumerable<PriceModel>>> GetAll()
        {
            var prices = await base.GetAll();

            if (!prices.Any())
            {
                return new NotFoundObjectResult("Цены не найдены в БД");
            }

            return new OkObjectResult(_mapper.Map<IEnumerable<PriceModel>>(prices));
        }

        public async Task<ActionResult<PriceModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new BadRequestObjectResult("Отсутствует идентификатор стоимости");
            }

            var price = await base.Get(id);

            if (price == null)
            {
                return new NotFoundObjectResult("Стоимость не найдена в БД");
            }

            return new OkObjectResult(_mapper.Map<PriceModel>(price));
        }

        public async Task<ActionResult> Create(PriceModel price)
        {
            if (price == null)
            {
                return new BadRequestObjectResult("Отсутствует стоимость для добавления");
            }

            var priceEntity = _mapper.Map<PriceDbModel>(price);

            await base.Create(priceEntity);

            return new OkResult();
        }

        public async Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices)
        {
            if (!prices.Any())
            {
                return new BadRequestObjectResult("Отсутствуют цены для добавления");
            }

            var priceEntity = _mapper.Map<IEnumerable<PriceDbModel>>(prices);

            await base.CreateMany(priceEntity);

            return new OkResult();
        }

        public async Task<ActionResult> Update(PriceModel price)
        {
            if (price == null)
            {
                return new BadRequestObjectResult("Отсутствует стоимость для обновления");
            }

            var priceEntity = _mapper.Map<PriceDbModel>(price);

            await base.Update(priceEntity);

            return new OkResult();
        }

        public async Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices)
        {
            if (!prices.Any())
            {
                return new BadRequestObjectResult("Отсутствуют цены для обновления");
            }

            var priceEntity = _mapper.Map<IEnumerable<PriceDbModel>>(prices);

            await base.UpdateMany(priceEntity);

            return new OkResult();
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new BadRequestObjectResult("Отсутствует идентификатор стоимости для удаления");
            }

            await base.Delete(id);

            return new OkResult();
        }

        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            if (!ids.Any())
            {
                return new BadRequestObjectResult("Отсутствует идентификаторы цен для удаления");
            }

            await base.DeleteMany(ids);

            return new OkResult();
        }
    }
}