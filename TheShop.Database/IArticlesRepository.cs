using TheShop.Models.Entities;

namespace TheShop.Database
{
    public interface IArticlesRepository
    {
        Article Get(int id);

        Article Add(Article article);
    }
}
