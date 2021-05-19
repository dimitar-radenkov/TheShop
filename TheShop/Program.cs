using System;

using TheShop.Services;

namespace TheShop
{
	internal class Program
	{
		private static void Main()
		{
           

			var shopService = new ShopService();

			try
			{
				var order = shopService.Order(articleId: 1, maxPrice: 250);
				shopService.Sell(order, buyerId: 10);

				var order1 = shopService.Order(articleId: 1, maxPrice: 350);
				shopService.Sell(order1, buyerId: 10);
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