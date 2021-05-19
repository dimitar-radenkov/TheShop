using System;

using TheShop.Database;
using TheShop.Services;

namespace TheShop
{
    internal class Program
    {
        private static void Main()
        {
            var ordersRepository = new OrdersRepository();
            var articlesRepository = new ArticlesRepository();
            var salesRepository = new SalesRepository();
            var offersRepository = new OffersRepository();

            var suppliersProvider = new SuppliersProvider();
            var suppliersService = new SuppliersService(suppliersProvider);

            var shopService = new ShopService(
                ordersRepository,
                articlesRepository,
                salesRepository,
                offersRepository,
                suppliersService);

            try
            {
                var order = shopService.MakeOrder(articleId: 1, maxPrice: 10);
                if (order.Status == Models.OrderStatus.Fulfilled)
                {
                    shopService.Sell(order, buyerId: 10);
                }

                var order1 = shopService.MakeOrder(articleId: 1, maxPrice: 20);
                if (order1.Status == Models.OrderStatus.Fulfilled)
                {
                    shopService.Sell(order1, buyerId: 10);
                }

                var article1 = shopService.GetById(articleId: 1);
                var article2 = shopService.GetById(articleId: 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}