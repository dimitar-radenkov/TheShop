using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Database
{
    public class OffersRepository : IOffersRepository
    {
        private List<Offer> orderArticles;

        public OffersRepository()
        {
            this.orderArticles = new List<Offer>();
        }

        public Offer Add(Offer orderArticle)
        {
            var id = this.orderArticles.Any() ? this.orderArticles.Max(x => x.Id) + 1 : 1;
            orderArticle.Id = id;

            this.orderArticles.Add(orderArticle);

            return orderArticle;
        }

        public Offer Get(int id)
        {
            var orderArticle = this.orderArticles.FirstOrDefault(x => x.Id == id);

            if (orderArticle == null)
            {
                throw new RepositoryException();
            }

            return orderArticle;
        }

        public IEnumerable<Offer> GetAll()
        {
            return this.orderArticles;
        }
    }
}
