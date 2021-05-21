using System.Collections.Generic;

using TheShop.Models.Entities;

namespace TheShop.Database
{
    public interface IOffersRepository
    {
        Offer Add(Offer orderArticle);
        Offer Get(int id);
        IEnumerable<Offer> GetAll();
    }
}
