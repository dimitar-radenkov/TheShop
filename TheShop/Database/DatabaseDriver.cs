using System.Collections.Generic;
using System.Linq;

namespace TheShop.Database
{
    public class DatabaseDriver
    {
        private readonly List<Article> articles;

        public DatabaseDriver()
        {
            this.articles = new List<Article>();
        }

        public Article Get(int id)
        {
            return this.articles.Single(x => x.ID == id);
        }

        public Article Add(Article article)
        {
            this.articles.Add(article);

            return article;
        }
    }
}
