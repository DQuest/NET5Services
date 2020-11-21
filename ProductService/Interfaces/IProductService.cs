using System.Collections.Generic;
using ProductService.Models;

namespace ProductService.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetAll();

        ProductModel Get(long id);
    }
}