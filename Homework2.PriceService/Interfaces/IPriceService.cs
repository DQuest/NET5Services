using System.Collections.Generic;
using Homework2.PriceService.Models;

namespace Homework2.PriceService.Interfaces
{
    public interface IPriceService
    {
        IEnumerable<PriceModel> GetAll();

        PriceModel Get(long id);
    }
}