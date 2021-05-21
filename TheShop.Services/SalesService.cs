using System;

using Serilog;

using TheShop.Database;
using TheShop.Models.Entities;

namespace TheShop.Services
{
    public class SalesService : ISalesService
    {
        private readonly ILogger logger;
        private readonly IOrdersRepository ordersRepository;
        private readonly ISalesRepository salesRepository;

        public SalesService(
            ILogger logger,
            IOrdersRepository ordersRepository,
            ISalesRepository salesRepository)
        {
            this.logger = logger;
            this.ordersRepository = ordersRepository;
            this.salesRepository = salesRepository;
        }

        public void Sell(int orderId, int offerId, int buyerId)
        {
            try
            {
                this.logger.Debug($"Processing order: {orderId} ");

                var order = this.ordersRepository.Get(orderId);

                var sale = new Sale
                {
                    OrderId = order.Id,
                    OfferId = offerId,
                    BuyerId = buyerId,
                    DateSold = DateTime.UtcNow,
                };

                this.salesRepository.Add(sale);

                order.Status = OrderStatus.Completed;
                this.ordersRepository.Update(order.Id, order);

                this.logger.Information($"Sold article {order.ArticleId} from {buyerId} according to {offerId}");
            }
            catch (Exception e)
            {
                this.logger.Error($"Unable to process order {orderId}, from {buyerId} according to {offerId}", e.Message);
                throw new ServiceException("Some error occured while processing order", e);
            }
        }
    }
}