using TheShop.Models;

namespace TheShop
{
    public interface ISupplier
    {
        bool HasArticle(int id);
        Article GetArticle(int id);
    }
}
