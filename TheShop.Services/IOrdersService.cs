using TheShop.Models;

namespace TheShop.Services
{
    public interface IOrdersService
    {
        OrderOffer GetOrder(int articleId, decimal maxPrice);
    }
}