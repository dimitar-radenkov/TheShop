using System.Collections.Generic;
using System.Linq;

using TheShop.Models;
using TheShop.Suppliers;

namespace TheShop.Services
{
    public class SuppliersService : ISuppliersService
    {
        public IEnumerable<ISupplier> suppliers;

        public SuppliersService(ISuppliersProvider suppliersProvider)
        {
            this.suppliers = suppliersProvider.GetSuppliers();
        }

        public IEnumerable<ArticleWithPrice> GetArticles(int id) =>
            this.suppliers
                .Where(x => x.HasArticle(id))
                .Select(x => x.GetArticle(id))
                .ToList();
    }
}
