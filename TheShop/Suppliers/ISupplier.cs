using TheShop.Models;

namespace TheShop.Suppliers
{
    public interface ISupplier
    {
        bool HasArticle(int id);
        ArticleWithPrice GetArticle(int id);
    }
}
