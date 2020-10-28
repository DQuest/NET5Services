namespace Homework2.PriceService.Interfaces
{
    using System.Collections.Generic;
    using Homework2.PriceService.Models;

    public interface IPriceController
    {
        IEnumerable<PriceModel> GetAll();

        PriceModel Get(long id);
    }
}