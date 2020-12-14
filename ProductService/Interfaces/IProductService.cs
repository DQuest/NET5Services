using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Interfaces
{
    public interface IProductService
    {
        Task<ActionResult<IEnumerable<ProductModel>>> GetAll();
        
        Task<ActionResult<ProductModel>> Get(Guid id);
        
        Task<ActionResult> Create(ProductModel product);

        Task<ActionResult> CreateMany(IEnumerable<ProductModel> products);

        Task<ActionResult> Update(ProductModel product);
        
        Task<ActionResult> UpdateMany(IEnumerable<ProductModel> products);

        Task<ActionResult> Delete(Guid id);

        Task<ActionResult> DeleteMany(IEnumerable<Guid> ids);
    }
}