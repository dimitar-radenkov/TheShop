using TheShop.Models;

namespace TheShop
{
    public class HiPriceSupplier : ISupplier
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
				Name = "Article from HiPriceSupplier",
				Price = 460
			};
		}
	}
}
