using AutoMapper;
using Homework2.ImageService.Entities;
using Homework2.ImageService.Models;

namespace Homework2.ImageService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ImageEntity, ImageModel>();
        }
    }
}