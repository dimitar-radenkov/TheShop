using System;

namespace TheShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderOffer
    {
        public int OrderId { get; set; }

        public int? OfferId { get; set; }

        public bool HasValidOffer => this.OfferId != null;
    }
}
