using TheShop.Models;

namespace TheShop.Suppliers
{
    public class LowPriceSupplier : ISupplier
    {
        private const int SUPPLIER_ID = 1;

        public bool HasArticle(int id)
        {
            return true;
        }

        public ArticleWithPrice GetArticle(int id)
        {
            return new ArticleWithPrice()
            {
                Id = id,
                SupplierId = SUPPLIER_ID,
                Name = "Article from LowPriceSupplier",
                Price = 458
            };
        }
    }
}
