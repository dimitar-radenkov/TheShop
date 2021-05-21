using System;

namespace TheShop.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
