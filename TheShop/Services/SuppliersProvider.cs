using System.Collections.Generic;

using TheShop.Suppliers;

namespace TheShop.Services
{
    public class SuppliersProvider : ISuppliersProvider
    {
        public IEnumerable<ISupplier> GetSuppliers()
        {
            return new List<ISupplier>
            {
                new LowPriceSupplier(),
                new MidPriceSupplier(),
                new HiPriceSupplier()
            };
        }
    }
}
