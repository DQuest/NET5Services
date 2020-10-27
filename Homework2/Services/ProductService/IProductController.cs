namespace Homework2.Services.ProductService
{
    using System.Collections.Generic;

    public interface IProductController
    {
        IEnumerable<ProductModel> GetAll();

        ProductModel Get(long id);
    }
}