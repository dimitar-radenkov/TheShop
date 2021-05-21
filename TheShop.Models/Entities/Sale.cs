using System;

namespace TheShop.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public DateTime DateSold { get; set; }
    }
}
