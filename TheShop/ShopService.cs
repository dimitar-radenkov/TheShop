using System;
using System.Linq;

using TheShop.Database;
using TheShop.Models;
using TheShop.Services;

namespace TheShop
{
    public class ShopService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IArticleRepository articleRepository;
        private readonly ISuppliersService suppliersService;
        private readonly Logger logger;

        public ShopService()
        {
            this.ordersRepository = new OrdersRepository();
            this.articleRepository = new ArticleRepository();
            this.suppliersService = new SuppliersService();

            this.logger = new Logger();
        }

        public OrderResult OrderArticle(int articleId, decimal maxPrice)
        {
            var order = new Order
            {
                Status = OrderStatus.WaitingFullfilment,
                ArticleId = articleId,
                DateCreated = DateTime.UtcNow,
            };

            order = this.ordersRepository.Add(order);

            var articlesFittingPrice = this.suppliersService.GetArticles(articleId)
                .Where(x => x.Price <= maxPrice)
                .OrderBy(x => x.Price)
                .ToList();

            if (!articlesFittingPrice.Any())
            {
                this.logger.Debug("No articles available that fits price limitation");
            }

            return new OrderResult(order.Id, articlesFittingPrice.FirstOrDefault());
        }

        public void SellArticle(OrderResult orderResult, int buyerId)
        {
            var article = orderResult.Article;
            if (article == null)
            {
                throw new ArgumentNullException("Could not order article");
            }

            this.logger.Debug("Trying to sell article with id=" + article.Id);

            var orderId = orderResult.OrderId;
            var order = this.ordersRepository.Get(orderId);
            order.BuyerId = buyerId;
            order.DateCompleted = DateTime.UtcNow;
            order.Status = OrderStatus.Completed;

            this.ordersRepository.Update(orderId, order);
            this.logger.Info("Article with id=" + article.Id + " is sold.");
        }

        public ArticleResult GetById(int id)
        {
            var article = this.articleRepository.Get(id);
            var orders = this.ordersRepository.GetAll().Where(x => x.ArticleId == id).ToList();

            return new ArticleResult { Article = article, Orders = orders };
        }
    }
}