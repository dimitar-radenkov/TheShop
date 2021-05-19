using System;
using System.Linq;

using TheShop.Database;
using TheShop.Models;

namespace TheShop.Services
{
    public class ShopService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IArticleRepository articleRepository;
        private readonly ISalesRepository salesRepository;
        private readonly IOrderOffersRepository offersRepository;
        private readonly ISuppliersService suppliersService;
        private readonly Logger logger;

        public ShopService()
        {
            this.ordersRepository = new OrdersRepository();
            this.articleRepository = new ArticleRepository();
            this.articleRepository.Add(new Article { Id = 1, Name = "Item1" });
            this.articleRepository.Add(new Article { Id = 2, Name = "Item2" });
            this.articleRepository.Add(new Article { Id = 3, Name = "Item3" });

            this.salesRepository = new SalesRepository();
            this.offersRepository = new OrderOffersRepository();
            this.suppliersService = new SuppliersService();

            this.logger = new Logger();
        }

        public Order Order(int articleId, decimal maxPrice)
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

                if (articles.Any())
                {
                    this.ordersRepository.Add(order);

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
                this.logger.Debug($"Unable to get article [{articleId}] from suppliers. Reason: {e.Message}");
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

            this.logger.Info("Article with id=" + order.ArticleId + " is sold.");
        }

        public Article GetById(int articleId)
        {
            var article = this.articleRepository.Get(articleId);

            var orders = this.ordersRepository.GetAll()
                .Where(x => x.ArticleId == articleId)
                .Where(x => x.Status == OrderStatus.Completed)
                .ToList();

            var sales = this.salesRepository.GetAll()
                .Where(sale => orders.Select(x => x.Id).ToList().Contains(sale.OrderId))
                .ToList();

            return article;
        }
    }
}