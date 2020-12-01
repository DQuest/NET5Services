using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();

        Task<ProductModel> GetProduct(Guid productId);

        Task CreateProduct(ProductModel product);

        Task UpdateProduct(ProductModel product);

        Task DeleteProducts(IEnumerable<Guid> productsIds);
    }
}