using TheShop.Models;

namespace TheShop.Suppliers
{
    public class MidPriceSupplier : ISupplier
    {
        private const int SUPPLIER_ID = 2;

        public bool HasArticle(int id)
        {
            return true;
        }

        public ArticleWithPrice GetArticle(int id)
        {
            return new ArticleWithPrice()
            {
                Id = 1,
                SupplierId = SUPPLIER_ID,
                Name = "Article from MidPriceSupplier",
                Price = 458
            };
        }
    }
}
