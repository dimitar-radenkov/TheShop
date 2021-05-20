using TheShop.Models;

namespace TheShop.Services
{
    public interface IOrdersService
    {
        Order GetOrder(int articleId, decimal maxPrice);
    }
}