using TheShop.Models;

namespace TheShop
{
    public class LowPriceSupplier : ISupplier
    {
		public bool HasArticle(int id)
		{
			return true;
		}

		public ArticleWithPrice GetArticle(int id)
		{
			return new ArticleWithPrice()
			{
				Id = 1,
				Name = "Article from LowPriceSupplier",
				Price = 458
			};
		}
	}
}
