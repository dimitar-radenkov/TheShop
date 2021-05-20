using System;

using Serilog.Core;

using TheShop.Services;

namespace TheShop
{

    public class Application : IApplication
    {
        private readonly Logger logger;
        private readonly IOrdersService ordersService;
        private readonly ISalesService salesService;

        public Application(
            Logger logger,
            IOrdersService ordersService,
            ISalesService salesService)
        {
            this.logger = logger;
            this.ordersService = ordersService;
            this.salesService = salesService;
        }

        public void Run()
        {
            try
            {
                var orderOffer = ordersService.GetOrder(articleId: 1, maxPrice: 100);
                if (orderOffer.HasValidOffer)
                {
                    salesService.Sell(orderOffer.OrderId, orderOffer.OfferId.Value, 1);
                }

                //var order = shopService.MakeOrder(articleId: 1, maxPrice: 10);
                //if (order.Status == Models.OrderStatus.Fulfilled)
                //{
                //    shopService.Sell(order, buyerId: 10);
                //}

                //var order1 = shopService.MakeOrder(articleId: 1, maxPrice: 20);
                //if (order1.Status == Models.OrderStatus.Fulfilled)
                //{
                //    shopService.Sell(order1, buyerId: 10);
                //}

                //var article1 = shopService.GetByArticleId(articleId: 1);
                //var article2 = shopService.GetByArticleId(articleId: 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
