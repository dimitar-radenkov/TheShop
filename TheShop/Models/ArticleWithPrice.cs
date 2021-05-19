namespace TheShop.Models
{
    public class ArticleWithPrice : Article
    {
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
    }
}
