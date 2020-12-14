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
    public class PriceRepository : BaseRepository<PriceDbModel>, IPriceRepository
    {
        private static string TableName = "Price";

        private readonly IMapper _mapper;

        public PriceRepository(IOptions<PriceDbOptions> dbOptions, IMapper mapper)
            : base(dbOptions, TableName)
        {
            _mapper = mapper;
        }

        public new async Task<ActionResult<IEnumerable<PriceModel>>> GetAll()
        {
            var prices = await base.GetAll();

            if (!prices.Any())
            {
                return new NotFoundObjectResult("Цены не найдены в БД");
            }

            // gives emphasis of the content that is returned
            return new ObjectResult(_mapper.Map<IEnumerable<PriceModel>>(prices));
        }

        public new async Task<ActionResult<PriceModel>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Отсутствует идентификатор стоимости");
            }

            var price = await base.Get(id);

            if (price == null)
            {
                return new NotFoundObjectResult("Стоимость не найдена в БД");
            }

            return _mapper.Map<PriceModel>(price);
        }

        public async Task<ActionResult> Create(PriceModel price)
        {
            if (price == null)
            {
                return new NotFoundObjectResult("Отсутствует стоимость для добавления");
            }

            var priceEntity = _mapper.Map<PriceDbModel>(price);

            FillBaseFieldsForCreateOperation(priceEntity);

            await base.Create(priceEntity);

            return new NoContentResult();
        }

        public async Task<ActionResult> CreateMany(IEnumerable<PriceModel> prices)
        {
            if (!prices.Any())
            {
                return new NotFoundObjectResult("Отсутствуют цены для добавления");
            }

            var priceEntity = _mapper.Map<IEnumerable<PriceDbModel>>(prices);

            foreach (var price in priceEntity)
            {
                FillBaseFieldsForCreateOperation(price);
            }

            await base.CreateMany(priceEntity);

            return new NoContentResult();
        }

        public async Task<ActionResult> Update(PriceModel price)
        {
            if (price == null)
            {
                return new NotFoundObjectResult("Отсутствует стоимость для обновления");
            }

            var priceEntity = _mapper.Map<PriceDbModel>(price);

            await base.Update(priceEntity);

            return new NoContentResult();
        }

        public async Task<ActionResult> UpdateMany(IEnumerable<PriceModel> prices)
        {
            if (!prices.Any())
            {
                return new NotFoundObjectResult("Отсутствуют цены для обновления");
            }

            var priceEntity = _mapper.Map<IEnumerable<PriceDbModel>>(prices);

            await base.UpdateMany(priceEntity);

            return new NoContentResult();
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundObjectResult("Отсутствует идентификатор стоимости для удаления");
            }

            await base.Delete(id, GetUncheckIsLastPropForDeletedQuery());

            return new NoContentResult();
        }

        public async Task<ActionResult> DeleteMany(IEnumerable<Guid> ids)
        {
            if (!ids.Any())
            {
                return new NotFoundObjectResult("Отсутствует идентификаторы цен для удаления");
            }

            await base.DeleteMany(ids, GetUncheckIsLastPropForDeletedQuery());

            return new NoContentResult();
        }

        private string GetUncheckIsLastPropForDeletedQuery()
        {
            return $"UPDATE {TableName} " +
                   $"SET IsLast = 0 ";
        }

        private void FillBaseFieldsForCreateOperation(PriceDbModel priceEntity)
        {
            priceEntity.Id = new Guid();
            priceEntity.IsLast = true;
            priceEntity.IsDeleted = false;
        }
    }
}