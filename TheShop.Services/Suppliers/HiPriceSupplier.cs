using TheShop.Models;

namespace TheShop.Suppliers
{
    public class HiPriceSupplier : ISupplier
    {
        private const int SUPPLIER_ID = 3;

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
                Price = 400
            };
        }
    }
}
