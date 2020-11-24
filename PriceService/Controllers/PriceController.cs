using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceService.Interfaces;
using PriceService.Models;
using PriceService.Repositories;

namespace PriceService.Controllers
{
    [ApiController]
    [Route("api/prices")]
    public class PriceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPriceRepository _priceRepository;

        public PriceController(IMapper mapper, IPriceRepository priceRepository)
        {
            _mapper = mapper;
            _priceRepository = priceRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<PriceModel>> GetAll()
        {
            var priceEntity = await _priceRepository.GetAll();
            return _mapper.Map<IEnumerable<PriceModel>>(priceEntity);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<PriceModel> Get(Guid id)
        {
            var priceEntity = await _priceRepository.Get(id);
            return _mapper.Map<PriceModel>(priceEntity);
        }

        [Authorize]
        [HttpPost]
        public async Task Create(PriceModel price)
        {
            var priceEntity = _mapper.Map<PriceDbModel>(price);
            await _priceRepository.Create(priceEntity);
        }

        [Authorize]
        [HttpPut]
        public async Task Update(PriceModel price)
        {
            var priceEntity = _mapper.Map<PriceDbModel>(price);
            await _priceRepository.Update(priceEntity);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _priceRepository.Delete(id);
        }
    }
}