using TheShop.Models;

namespace TheShop.Services
{
    public interface ISalesService
    {
        void Sell(Order order, int offerId, int buyerId);
    }
}