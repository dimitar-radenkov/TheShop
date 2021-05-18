using System;

namespace TheShop
{
	internal class Program
	{
		private static void Main()
		{
			var shopService = new ShopService();

			try
			{
				var orderResult = shopService.OrderArticle(articleId: 1, maxPrice: 20);
				shopService.SellArticle(orderResult, buyerId: 10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			try
			{
				//print article on console
				var article = shopService.GetById(1);
				Console.WriteLine("Found article with ID: " + article.Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			try
			{
				//print article on console				
				var article = shopService.GetById(12);
				Console.WriteLine("Found article with ID: " + article.Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			Console.ReadKey();
		}
	}
}