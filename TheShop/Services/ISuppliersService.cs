using System.Collections.Generic;

using TheShop.Models;

namespace TheShop.Services
{
    public interface ISuppliersService
    {
        IEnumerable<Article> GetArticles(int id);
    }
}
