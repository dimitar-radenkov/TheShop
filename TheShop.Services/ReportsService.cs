﻿using System;
using System.Linq;

using Serilog;

using TheShop.Database;
using TheShop.Models.Entities;
using TheShop.Models.ViewModels;

namespace TheShop.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ILogger logger;
        private readonly IArticlesRepository articlesRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly ISalesRepository salesRepository;
        private readonly IOffersRepository offersRepository;

        public ReportsService(
            ILogger logger,
            IArticlesRepository articlesRepository,
            IOrdersRepository ordersRepository,
            ISalesRepository salesRepository,
            IOffersRepository offersRepository)
        {
            this.logger = logger;
            this.articlesRepository = articlesRepository;
            this.ordersRepository = ordersRepository;
            this.salesRepository = salesRepository;
            this.offersRepository = offersRepository;
        }

        public ReportViewModel GetByArticleId(int articleId)
        {
            try
            {
                this.logger.Debug($"Getting information for article : {articleId}");

                var article = this.articlesRepository.Get(articleId);

                var orders = this.ordersRepository.GetAll()
                    .Where(x => x.ArticleId == articleId)
                    .Where(x => x.Status == OrderStatus.Completed)
                    .ToList();

                var sales = this.salesRepository.GetAll()
                    .Where(sale => orders.Select(x => x.Id).Contains(sale.OrderId))
                    .Select(sale => new SaleViewModel
                    {
                        Price = this.offersRepository.Get(sale.OfferId).Price,
                        Buyer = sale.BuyerId.ToString(),
                        DateSold = sale.DateSold,
                        DateOrdered = orders.FirstOrDefault(x => x.Id == sale.OrderId).DateCreated,
                    })
                    .ToList();

                return new ReportViewModel(article.Id, article.Name, sales);
            }
            catch (Exception ex)
            {
                this.logger.Error($"Some error occured while getting information for article {articleId}.\n{ex.StackTrace}");
                throw new ServiceException(ex.Message);
            }
        }
    }
}