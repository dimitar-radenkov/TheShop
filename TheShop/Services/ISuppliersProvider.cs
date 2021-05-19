using System.Collections.Generic;

using TheShop.Suppliers;

namespace TheShop.Services
{
    public interface ISuppliersProvider
    {
        IEnumerable<ISupplier> GetSuppliers();
    }
}
