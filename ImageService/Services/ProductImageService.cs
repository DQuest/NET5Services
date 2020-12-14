using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageService.Interfaces;
using ImageService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly ImageContext _imageContext;
        private readonly IMapper _mapper;

        public ProductImageService(ImageContext imageContext, IMapper mapper)
        {
            _imageContext = imageContext;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<ImageModel>>> GetAllImagesForProduct(Guid productId)
        {
            // todo: оптимизировать (paging?, asAsyncEnum?) 
            var images = await _imageContext.Image
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            if (!images.Any())
            {
                return new NotFoundObjectResult($"Изображения для продукта с идентификатором {productId} не найдены в БД");
            }

            // gives emphasis of the content that is returned
            return new ObjectResult(_mapper.Map<IEnumerable<ImageModel>>(images));
        }
    }
}