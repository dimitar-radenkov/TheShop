using System.Collections.Generic;

using TheShop.Models.Entities;

namespace TheShop.Database
{
    public interface ISalesRepository
    {
        Sale Add(Sale sale);
        Sale Get(int id);
        IEnumerable<Sale> GetAll();
    }
}
