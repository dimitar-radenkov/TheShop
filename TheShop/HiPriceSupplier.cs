using TheShop.Models;

namespace TheShop
{
    public class HiPriceSupplier : ISupplier
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
				Name = "Article from HiPriceSupplier",
				Price = 460
			};
		}
	}
}
