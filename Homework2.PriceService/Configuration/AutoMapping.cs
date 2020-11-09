using AutoMapper;
using Homework2.PriceService.Models;
using Homework2.PriceService.Repositories;

namespace Homework2.PriceService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PriceDbModel, PriceModel>();
            CreateMap<PriceModel, PriceDbModel>();
        }
    }
}