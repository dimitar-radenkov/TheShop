using TheShop.Models;

namespace TheShop
{
    public class MidPriceSupplier : ISupplier
	{
		public bool HasArticle(int id)
		{
			return true;
		}

		public ArticleWithPrice GetArticle(int id)
		{
			return new ArticleWithPrice()
			{
				Id = id,
				Name = "Article from MidPriceSupplier",
				Price = 459
			};
		}
	}
}
