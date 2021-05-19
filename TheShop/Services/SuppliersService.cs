using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Services
{
    public class SuppliersService : ISuppliersService
    {
        public IEnumerable<ISupplier> suppliers;

        public SuppliersService()
        {
            this.suppliers = new List<ISupplier>
            {
                new LowPriceSupplier(),
                new MidPriceSupplier(),
                new HiPriceSupplier()
            };
        }

        public IEnumerable<ArticleWithPrice> GetArticles(int id)
        {
            return this.suppliers
                .Where(x => x.HasArticle(id))
                .Select(x => x.GetArticle(id))
                .ToList();
        }
    }
}
