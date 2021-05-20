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

        public OrderOffer GetOrder(int articleId, decimal maxPrice)
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
                this.ordersRepository.Update(order.Id, order);

                int? bestOfferId = null;
                if (articles.Any())
                {
                    var offers = articles
                        .Select(x => new Offer
                        {
                            ArticleId = articleId,
                            OrderId = order.Id,
                            SupplierId = x?.SupplierId,
                            Price = x?.Price
                        })
                        .OrderBy(x => x.Price)
                        .ToList();

                    offers.ForEach(x => this.offersRepository.Add(x));

                    bestOfferId = offers.First().Id;
                }

                return new OrderOffer { OrderId = order.Id, OfferId = bestOfferId };
            }
            catch (Exception e)
            {
                this.logger.Debug($"Unable to make order for article [{articleId}]. Reason: {e.Message}");

                return new OrderOffer { OrderId = order.Id, OfferId = null };
            }
        }
    }
}