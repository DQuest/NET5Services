namespace Homework2.Services.PriceService
{
    using System.Collections.Generic;

    public interface IPriceController
    {
        IEnumerable<PriceModel> GetAll();

        PriceModel Get(long id);
    }
}