using AutoMapper;
using ImageService.Entities;
using ImageService.Models;

namespace ImageService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ImageEntity, ImageModel>();
            CreateMap<ImageModel, ImageEntity>();
        }
    }
}