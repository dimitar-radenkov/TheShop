using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Database
{
    public class ArticleRepository : IArticleRepository
    {
        private IList<Article> articles;

        public ArticleRepository()
        {
            this.articles = new List<Article>();
        }

        public Article Add(string name, decimal price)
        {
            var id = this.articles.Any() ? this.articles.Max(x => x.Id) + 1 : 1;
            var article = new Article
            {
                Id = id,
                Name = name,
                Price = price
            };

            this.articles.Add(article);

            return article;
        }

        public Article Get(int id)
        {
            return this.articles.Single(x => x.Id == id);
        }
    }
}
