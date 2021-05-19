using System.Collections.Generic;

using TheShop.Models;

namespace TheShop.Database
{
    public interface ISalesRepository
    {
        Sale Add(Sale sale);

        IEnumerable<Sale> GetAll();
    }
}
