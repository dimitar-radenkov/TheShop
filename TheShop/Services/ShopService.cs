using System;
using System.Linq;

using Serilog.Core;

using TheShop.Database;
using TheShop.Models;
using TheShop.Models.ViewModels;

namespace TheShop.Services
{
    public class ShopService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IArticlesRepository articlesRepository;
        private readonly ISalesRepository salesRepository;
        private readonly IOffersRepository offersRepository;
        private readonly ISuppliersService suppliersService;
        private readonly Logger logger;

        public ShopService(
            Logger logger,
            IOrdersRepository ordersRepository,
            IArticlesRepository articlesRepository,
            ISalesRepository salesRepository,
            IOffersRepository offersRepository,
            ISuppliersService suppliersService)
        {
            this.logger = logger;
            this.ordersRepository = ordersRepository;
            this.articlesRepository = articlesRepository;
            this.salesRepository = salesRepository;
            this.offersRepository = offersRepository;
            this.suppliersService = suppliersService;
        }

        public Order MakeOrder(int articleId, decimal maxPrice)
        {
            var order = new Order
            {
                ArticleId = articleId,
                MaxPrice = maxPrice,
                Status = OrderStatus.AwaitingFulfillment,
                DateCreated = DateTime.UtcNow,
            };

            try
            {
                var articles = this.suppliersService.GetArticles(articleId)
                    .Where(x => x.Price <= maxPrice)
                    .ToList();

                order.Status = articles.Any() ? OrderStatus.Fulfilled : OrderStatus.Unfullfilled;

                this.ordersRepository.Add(order);

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

        public void Sell(Order order, int buyerId)
        {
            this.logger.Debug("Trying to sell article with id=" + order.ArticleId);

            var bestOffer = this.offersRepository.GetAll()
                .Where(x => x.OrderId == order.Id)
                .OrderBy(x => x.Price)
                .ToList()
                .FirstOrDefault();

            var sale = new Sale
            {
                OrderId = order.Id,
                OfferId = bestOffer.Id,
                BuyerId = buyerId,
                DateSold = DateTime.UtcNow,
            };

            this.salesRepository.Add(sale);

            order.Status = OrderStatus.Completed;
            this.ordersRepository.Update(order.Id, order);

            this.logger.Information("Article with id=" + order.ArticleId + " is sold.");
        }

        public ArticleViewModel GetById(int articleId)
        {
            var article = this.articlesRepository.Get(articleId);

            var orders = this.ordersRepository.GetAll()
                .Where(x => x.ArticleId == articleId)
                .Where(x => x.Status == OrderStatus.Completed)
                .ToList();

            var sales = this.salesRepository.GetAll()
                .Where(sale => orders.Select(x => x.Id).ToList().Contains(sale.OrderId))
                .ToList();

            return new ArticleViewModel(article.Id, article.Name);
        }
    }
}