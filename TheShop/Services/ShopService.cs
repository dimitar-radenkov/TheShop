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
        private readonly ISuppliersService suppliersService;
        private readonly Logger logger;

        public ShopService()
        {
            this.ordersRepository = new OrdersRepository();
            this.articleRepository = new ArticleRepository();
            this.suppliersService = new SuppliersService();

            this.logger = new Logger();
        }

        public int OrderArticle(int articleId, decimal maxPrice)
        {
            var order = new Order
            {
                Status = OrderStatus.Created,
                DateCreated = DateTime.UtcNow,
            };

            order = this.ordersRepository.Add(order);

            var articlesFittingPrice = this.suppliersService.GetArticles(articleId)
                .Where(x => x.Price <= maxPrice)
                .OrderBy(x => x.Price)
                .ToList();

            if (!articlesFittingPrice.Any())
            {
                this.logger.Debug("No articles available");
            }

            return order.Id;
        }

        public void SellArticle(int orderId, int buyerId)
        {
            var order = this.ordersRepository.Get(orderId);
            var article = this.articleRepository.Get(order.ArticleId);
            if (article == null)
            {
                throw new ArgumentNullException("Could not order article");
            }

            this.logger.Debug("Trying to sell article with id=" + article.Id);

            order.BuyerId = buyerId;
            order.DateCompleted = DateTime.UtcNow;
            order.Status = OrderStatus.Completed;

            this.ordersRepository.Update(orderId, order);
            this.logger.Info("Article with id=" + article.Id + " is sold.");
        }

        public Article GetById(int id)
        {
            var article = this.articleRepository.Get(id);
            var orders = this.ordersRepository.GetAll().Where(x => x.ArticleId == id).ToList();

            return article;
        }
    }
}