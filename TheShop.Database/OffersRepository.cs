using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Database
{
    public class OffersRepository : IOffersRepository
    {
        private List<OrderOffer> orderArticles;

        public OffersRepository()
        {
            this.orderArticles = new List<OrderOffer>();
        }

        public OrderOffer Add(OrderOffer orderArticle)
        {
            var id = this.orderArticles.Any() ? this.orderArticles.Max(x => x.Id) + 1 : 1;
            orderArticle.Id = id;

            this.orderArticles.Add(orderArticle);

            return orderArticle;
        }

        public OrderOffer Get(int id)
        {
            var orderArticle = this.orderArticles.FirstOrDefault(x => x.Id == id);

            if (orderArticle == null)
            {
                throw new RepositoryException();
            }

            return orderArticle;
        }

        public IEnumerable<OrderOffer> GetAll()
        {
            return this.orderArticles;
        }
    }
}
