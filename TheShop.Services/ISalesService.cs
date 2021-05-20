namespace TheShop.Services
{
    public interface ISalesService
    {
        void Sell(int orderId, int offerId, int buyerId);
    }
}