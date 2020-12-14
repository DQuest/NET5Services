using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Interfaces
{
    public interface IProductImageService
    {
        Task<ActionResult<IEnumerable<ImageModel>>> GetAllImagesForProduct(Guid productId);
    }
}