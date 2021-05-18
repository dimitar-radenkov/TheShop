using System;

namespace TheShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int BuyerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        public OrderStatus Status { get; set; }
    }
}
