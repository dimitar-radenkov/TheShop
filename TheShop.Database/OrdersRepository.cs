using System.Collections.Generic;
using System.Linq;

using TheShop.Models.Entities;

namespace TheShop.Database
{
    public class OrdersRepository : IOrdersRepository
    {
        private const string UNKNONW_ID = "Unknown Order Id";
        private readonly Dictionary<int, Order> orders;

        public OrdersRepository()
        {
            this.orders = new Dictionary<int, Order>();
        }

        public Order Add(Order order)
        {
            var id = this.orders.Any() ? this.orders.Max(x => x.Key) + 1 : 1;
            order.Id = id;

            this.orders.Add(id, order);

            return order;
        }

        public Order Get(int id)
        {
            if (!this.orders.ContainsKey(id))
            {
                throw new RepositoryException(UNKNONW_ID);
            }

            return this.orders[id];
        }

        public IEnumerable<Order> GetAll()
        {
            return this.orders.Values;
        }

        public void Update(int orderId, Order updatedOrder)
        {
            if (!this.orders.ContainsKey(orderId))
            {
                throw new RepositoryException(UNKNONW_ID);
            }

            this.orders[orderId] = updatedOrder;
        }
    }
}
