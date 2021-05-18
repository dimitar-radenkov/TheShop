using TheShop.Models;

namespace TheShop.Database
{
    public interface IArticleRepository
    {
        Article Get(int id);

        Article Add(string name, decimal price);
    }
}
