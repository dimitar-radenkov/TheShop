using System.Collections.Generic;

using TheShop.Models;

namespace TheShop.Database
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetAll();

        Order Add(int articleId, int buyerId, OrderStatus status);
    }
}
