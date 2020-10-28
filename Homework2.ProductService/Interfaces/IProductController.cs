namespace Homework2.Interfaces
{
    using System.Collections.Generic;
    using Homework2.Models;

    public interface IProductController
    {
        IEnumerable<ProductModel> GetAll();

        ProductModel Get(long id);
    }
}