using TheShop.Models;

namespace TheShop
{
    public class MidPriceSupplier : ISupplier
	{
		public bool HasArticle(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				Id = id,
				Name = "Article from MidPriceSupplier",
				Price = 459
			};
		}
	}
}
