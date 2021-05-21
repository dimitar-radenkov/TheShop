using System.Collections.Generic;
using System.Linq;

using TheShop.Models.Entities;

namespace TheShop.Database
{
    public class ArticlesRepository : IArticlesRepository
    {
        private IList<Article> articles;

        public ArticlesRepository()
        {
            this.articles = new List<Article>();
        }

        public Article Add(Article article)
        {
            if (article == null)
            {
                throw new RepositoryException(nameof(article));
            }

            var id = this.articles.Any() ? this.articles.Max(x => x.Id) + 1 : 1;
            article.Id = id;

            this.articles.Add(article);

            return article;
        }

        public Article Get(int id)
        {
            var article = this.articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                throw new RepositoryException("Unable to find article");
            }

            return article;
        }
    }
}
