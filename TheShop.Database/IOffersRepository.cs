using System.Collections.Generic;

using TheShop.Models;

namespace TheShop.Database
{
    public interface IOffersRepository
    {
        OrderOffer Add(OrderOffer orderArticle);
        OrderOffer Get(int id);
        IEnumerable<OrderOffer> GetAll();
    }
}
