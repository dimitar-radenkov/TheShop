using TheShop.Models;

namespace TheShop.Database
{
    public interface IArticlesRepository
    {
        Article Get(int id);

        Article Add(Article article);
    }
}
