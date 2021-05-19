namespace TheShop.Models
{
    public class OrderOffer
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public int? SupplierId { get; set; }
        public decimal? Price { get; set; }
    }
}
