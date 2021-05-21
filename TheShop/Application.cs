using System;

using Serilog;

using TheShop.Services;

namespace TheShop
{

    public class Application : IApplication
    {
        private readonly ILogger logger;
        private readonly IOrdersService ordersService;
        private readonly ISalesService salesService;
        private readonly IReportsService reportsService;

        public Application(
            ILogger logger,
            IOrdersService ordersService,
            ISalesService salesService,
            IReportsService reportsService)
        {
            this.logger = logger;
            this.ordersService = ordersService;
            this.salesService = salesService;
            this.reportsService = reportsService;
        }

        public void Run()
        {
            try
            {
                var quit = false;
                while (!quit)
                {
                    Console.WriteLine("1. Order");
                    Console.WriteLine("2. Get Information");
                    Console.WriteLine("3. Quit");
                    Console.WriteLine();

                    var choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            {
                                var articleId = this.AskForArticleId();
                                var maxPrice = this.AskForMaxPrice();
                                this.logger.Debug($"Make order for article: {articleId} with max price: {maxPrice} ");
                                var orderOffer = this.ordersService.GetOrder(articleId, maxPrice);
                                if (orderOffer.HasValidOffer)
                                {
                                    salesService.Sell(orderOffer.OrderId, orderOffer.OfferId.Value, 1);
                                }

                                break;
                            }
                        case 2:
                            {
                                var articleId = this.AskForArticleId();
                                this.logger.Debug($"Get information for article: {articleId} ");
                                break;
                            }
                        case 3:
                            {
                                this.logger.Debug("Quiting application");
                                Console.WriteLine("Good bye");
                                quit = true;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Unknown command");
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Debug($"Unknown error occured: {ex.Message}");
                Console.WriteLine(ex);
            }
        }

        private int AskForArticleId()
        {
            Console.Write("Please enter article Id: ");
            var articleId = int.Parse(Console.ReadLine());

            return articleId;
        }

        private decimal AskForMaxPrice()
        {
            Console.Write("Please enter max price: ");
            var maxPrice = decimal.Parse(Console.ReadLine());

            return maxPrice;
        }
    }
}
