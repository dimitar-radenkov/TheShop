using System.Collections.Generic;

using TheShop.Models;

namespace TheShop.Database
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetAll();

        Order Add(Order order);

        Order Get(int id);

        void Update(int orderId, Order updatedOrder);
    }
}
