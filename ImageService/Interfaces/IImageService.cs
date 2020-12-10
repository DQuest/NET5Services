using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageService.Entities;
using ImageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Interfaces
{
    public interface IImageService
    {
        Task<ActionResult<IEnumerable<ImageModel>>> GetAll();
        
        Task<ActionResult<ImageModel>> Get(Guid id);
        
        Task<ActionResult> Create(ImageModel image);

        Task<ActionResult> CreateMany(IEnumerable<ImageModel> images);

        Task<ActionResult> Update(ImageModel image);
        
        Task<ActionResult> UpdateMany(IEnumerable<ImageModel> images);

        Task<ActionResult> Delete(Guid id);

        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}