using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAll();

        Task<ProductModel> Get(Guid productId);

        Task Create(ProductModel product);

        Task Update(ProductModel product);

        Task Delete(Guid productId);
    }
}