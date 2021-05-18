using TheShop.Models;

namespace TheShop
{
    public class OrderResult
    {
        public int OrderId { get; }

        public Article Article { get; }

        public OrderResult(int orderId, Article article)
        {
            this.OrderId = orderId;
            this.Article = article;
        }
    }
}