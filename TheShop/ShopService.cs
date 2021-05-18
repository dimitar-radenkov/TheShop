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
        private Logger logger;

        public ShopService()
        {
            this.ordersRepository = new OrdersRepository();
            this.articleRepository = new ArticleRepository();
            this.suppliersService = new SuppliersService();

            logger = new Logger();
        }

        public Article OrderArticle(int articleId, decimal maxPrice)
        {
            #region ordering article

            var allAvailbleArticles = this.suppliersService.GetArticles(articleId);
            if (!allAvailbleArticles.Any())
            {
                this.logger.Debug("No articles available");
            }

            var articlesFittingPrice = allAvailbleArticles
                .Where(x => x.Price <= maxPrice)
                .OrderBy(x => x.Price)
                .ToList();

            if (!articlesFittingPrice.Any())
            {
                this.logger.Debug("No articles available that fits price limitation");
            }

            var article = articlesFittingPrice.FirstOrDefault();
            return article;
        }

        public void SellArticle(Article article, int buyerId)
        {
            #region selling article

            if (article == null)
            {
                throw new Exception("Could not order article");
            }

            logger.Debug("Trying to sell article with id=" + article.Id);

            //article.IsSold = true;
            //article.SoldDate = DateTime.Now;
            //article.BuyerUserId = buyerId;

            try
            {
                this.articleRepository.Add(article.Name, article.Price);
                this.ordersRepository.Add(article.Id, buyerId, OrderStatus.Completed);
                logger.Info("Article with id=" + article.Id + " is sold.");
            }
            catch (ArgumentNullException ex)
            {
                logger.Error("Could not save article with id=" + article.Id);
                throw new Exception("Could not save article with id");
            }
            catch (Exception)
            {
            }

            #endregion
        }

        public Article GetById(int id)
        {
            return this.articleRepository.Get(id);
        }
    }
}
