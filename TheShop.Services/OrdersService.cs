﻿using System;
using System.Linq;

using Serilog;

using TheShop.Database;
using TheShop.Models;
using TheShop.Models.Entities;

namespace TheShop.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ILogger logger;
        private readonly IOrdersRepository ordersRepository;
        private readonly IOffersRepository offersRepository;
        private readonly ISuppliersService suppliersService;

        public OrdersService(
            ILogger logger,
            IOrdersRepository ordersRepository,
            IOffersRepository offersRepository,
            ISuppliersService suppliersService)
        {
            this.logger = logger;
            this.ordersRepository = ordersRepository;
            this.offersRepository = offersRepository;
            this.suppliersService = suppliersService;
        }

        public OrderOffer GetOrder(int articleId, decimal maxPrice)
        {
            try
            {
                this.logger.Debug($"Making an order for article {articleId} with max price: {maxPrice}");

                var order = new Order
                {
                    ArticleId = articleId,
                    MaxPrice = maxPrice,
                    Status = OrderStatus.AwaitingFulfillment,
                    DateCreated = DateTime.UtcNow,
                };

                order = this.ordersRepository.Add(order);

                var articles = this.suppliersService.GetArticles(articleId);
                articles.ToList().ForEach(x => this.logger.Debug($"Supplier:{x.SupplierId} sent offer:{x.Price} for article:{articleId}"));
                articles = articles.Where(x => x.Price <= maxPrice).ToList();

                order.Status = articles.Any() ? OrderStatus.Fulfilled : OrderStatus.Unfullfilled;
                this.ordersRepository.Update(order.Id, order);

                int? bestOfferId = null;
                if (articles.Any())
                {
                    var offers = articles
                        .Select(a => new Offer
                        {
                            ArticleId = articleId,
                            OrderId = order.Id,
                            SupplierId = a.SupplierId,
                            Price = a.Price
                        })
                        .OrderBy(x => x.Price)
                        .ToList();

                    offers.ForEach(o => this.offersRepository.Add(o));

                    bestOfferId = offers.First().Id;
                }

                return new OrderOffer { OrderId = order.Id, OfferId = bestOfferId };
            }
            catch (Exception e)
            {
                this.logger.Error($"Unable to make order for article [{articleId}]. Reason: {e.Message}");
                throw new ServiceException("Some error occured while ordering article", e);
            }
        }
    }
}