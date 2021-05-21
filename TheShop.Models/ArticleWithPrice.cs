using TheShop.Models.Entities;

namespace TheShop.Models
{
    public class ArticleWithPrice : Article
    {
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
    }
}
