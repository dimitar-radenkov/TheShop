using System;
using System.Collections.Generic;
using System.Linq;

using TheShop.Models;

namespace TheShop.Database
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IList<Order> orders;

        public OrdersRepository()
        {
            this.orders = new List<Order>();
        }

        public Order Add(int articleId, int buyerId, OrderStatus status)
        {
            var id = this.orders.Any() ? this.orders.Max(x => x.Id) + 1 : 1;
            var order = new Order
            {
                Id = id,
                ArticleId = articleId,
                BuyerId = buyerId,
                DateCreated = DateTime.UtcNow,
                DateCompleted = DateTime.UtcNow,
                Status = status
            };

            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            return this.orders;
        }
    }
}
