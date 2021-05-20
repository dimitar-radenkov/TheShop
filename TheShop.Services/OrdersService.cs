using System;
using System.Linq;

using Serilog.Core;

using TheShop.Database;
using TheShop.Models;

namespace TheShop.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly Logger logger;
        private readonly IOrdersRepository ordersRepository;
        private readonly IOffersRepository offersRepository;
        private readonly IArticlesRepository articlesRepository;
        private readonly ISuppliersService suppliersService;

        public OrdersService(
            Logger logger,
            IOrdersRepository ordersRepository,
            IOffersRepository offersRepository,
            IArticlesRepository articlesRepository,
            ISuppliersService suppliersService)
        {
            this.logger = logger;
            this.ordersRepository = ordersRepository;
            this.offersRepository = offersRepository;
            this.articlesRepository = articlesRepository;
            this.suppliersService = suppliersService;
        }

        public Order GetOrder(int articleId, decimal maxPrice)
        {
            var order = new Order
            {
                ArticleId = articleId,
                MaxPrice = maxPrice,
                Status = OrderStatus.AwaitingFulfillment,
                DateCreated = DateTime.UtcNow,
            };

            this.ordersRepository.Add(order);

            try
            {
                var articles = this.suppliersService.GetArticles(articleId)
                    .Where(x => x.Price <= maxPrice)
                    .ToList();

                order.Status = articles.Any() ? OrderStatus.Fulfilled : OrderStatus.Unfullfilled;

                if (articles.Any())
                {
                    articles
                        .Select(x => new OrderOffer
                        {
                            ArticleId = articleId,
                            OrderId = order.Id,
                            SupplierId = x?.SupplierId,
                            Price = x?.Price
                        })
                        .ToList()
                        .ForEach(oa => this.offersRepository.Add(oa));
                }

                return order;
            }
            catch (Exception e)
            {
                this.logger.Debug($"Unable to make order for article [{articleId}]. Reason: {e.Message}");
                order = this.ordersRepository.Add(order);

                return order;
            }
        }
    }
}