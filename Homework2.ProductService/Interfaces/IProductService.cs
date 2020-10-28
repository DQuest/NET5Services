using System.Collections.Generic;
using Homework2.ProductService.Models;

namespace Homework2.ProductService.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductModel> GetAll();

        ProductModel Get(long id);
    }
}