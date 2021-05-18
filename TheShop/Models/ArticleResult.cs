using System.Collections.Generic;

namespace TheShop.Models
{
    public class ArticleResult
    {
        public Article Article { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
