using System.Collections.Generic;
using System.Linq;

using TheShop.Models.Entities;

namespace TheShop.Database
{
    public class SalesRepository : ISalesRepository
    {
        private IList<Sale> sales;

        public SalesRepository()
        {
            this.sales = new List<Sale>();
        }

        public Sale Add(Sale sale)
        {
            if (sale == null)
            {
                throw new RepositoryException(nameof(sale));
            }

            var id = this.sales.Any() ? this.sales.Max(x => x.Id) + 1 : 1;
            sale.Id = id;

            this.sales.Add(sale);

            return sale;
        }

        public IEnumerable<Sale> GetAll()
        {
            return this.sales;
        }
    }
}
