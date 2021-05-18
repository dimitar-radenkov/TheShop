using TheShop.Models;

namespace TheShop
{
    public class Supplier1
	{
		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				Id = 1,
				Name = "Article from supplier1",
				Price = 458
			};
		}
	}
}
