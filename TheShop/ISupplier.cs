using TheShop.Models;

namespace TheShop
{
    public interface ISupplier
    {
        bool HasArticle(int id);
        ArticleWithPrice GetArticle(int id);
    }
}
