using TheShop.Models;

namespace TheShop
{
    public class LowPriceSupplier : ISupplier
    {
		public bool HasArticle(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				Id = 1,
				Name = "Article from LowPriceSupplier",
				Price = 458
			};
		}
	}
}
