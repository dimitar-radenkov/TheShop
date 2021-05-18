using System;

using TheShop.Database;
using TheShop.Models;

namespace TheShop
{
    public class ShopService
	{
		private IOrdersRepository ordersRepository;
		private IArticleRepository articleRepository;
		private Logger logger;

		private LowPriceSupplier Supplier1;
		private MidPriceSupplier Supplier2;
		private HiPriceSupplier Supplier3;
		
		public ShopService()
		{
			this.ordersRepository = new OrdersRepository();
			this.articleRepository = new ArticleRepository();
			logger = new Logger();
			Supplier1 = new LowPriceSupplier();
			Supplier2 = new MidPriceSupplier();
			Supplier3 = new HiPriceSupplier();
		}

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
		{
            #region ordering article

            Article article = null;
            Article tempArticle = null;
            var articleExists = Supplier1.HasArticle(id);
            if (articleExists)
            {
                tempArticle = Supplier1.GetArticle(id);
                if (maxExpectedPrice < tempArticle.Price)
                {
                    articleExists = Supplier2.HasArticle(id);
                    if (articleExists)
                    {
                        tempArticle = Supplier2.GetArticle(id);
                        if (maxExpectedPrice < tempArticle.Price)
                        {
                            articleExists = Supplier3.HasArticle(id);
                            if (articleExists)
                            {
                                tempArticle = Supplier3.GetArticle(id);
                                if (maxExpectedPrice < tempArticle.Price)
                                {
                                    article = tempArticle;
                                }
                            }
                        }
                    }
                }
            }

            article = tempArticle;
            #endregion

            #region selling article

            if (article == null)
            {
                throw new Exception("Could not order article");
            }

            logger.Debug("Trying to sell article with id=" + id);

            //article.IsSold = true;
            //article.SoldDate = DateTime.Now;
            //article.BuyerUserId = buyerId;

            try
            {
                this.articleRepository.Add(article.Name, article.Price);
                logger.Info("Article with id=" + id + " is sold.");
            }
            catch (ArgumentNullException ex)
            {
                logger.Error("Could not save article with id=" + id);
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
