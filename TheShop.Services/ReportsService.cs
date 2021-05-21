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

        public ReportsService(
            ILogger logger,
            IArticlesRepository articlesRepository,
            IOrdersRepository ordersRepository,
            ISalesRepository salesRepository)
        {
            this.logger = logger;
            this.articlesRepository = articlesRepository;
            this.ordersRepository = ordersRepository;
            this.salesRepository = salesRepository;
        }

        public ArticleViewModel GetByArticleId(int articleId)
        {
            var article = this.articlesRepository.Get(articleId);

            var orders = this.ordersRepository.GetAll()
                .Where(x => x.ArticleId == articleId)
                .Where(x => x.Status == OrderStatus.Completed)
                .ToList();

            var sales = this.salesRepository.GetAll()
                .Where(sale => orders.Select(x => x.Id).ToList().Contains(sale.OrderId))
                .ToList();

            return new ArticleViewModel(article.Id, article.Name);
        }
    }
}