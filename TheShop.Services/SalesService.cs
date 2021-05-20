using System;

using Serilog;

using TheShop.Database;
using TheShop.Models;

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
            var order = this.ordersRepository.Get(orderId);
            this.logger.Debug("Trying to sell article with id=" + order.ArticleId);

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

            this.logger.Information("Article with id=" + order.ArticleId + " is sold.");
        }
    }
}