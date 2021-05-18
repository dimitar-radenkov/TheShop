using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Services
{
    public interface ISuppliersService
    {
        IEnumerable<Article> GetArticles(int id);
    }

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

        public IEnumerable<Article> GetArticles(int id)
        {
            return this.suppliers
                .Where(x => x.HasArticle(id))
                .Select(x => x.GetArticle(id))
                .ToList();
        }
    }
}
