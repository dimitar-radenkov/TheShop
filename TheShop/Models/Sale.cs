using System;

namespace TheShop.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public DateTime DateSold { get; set; }
    }
}
