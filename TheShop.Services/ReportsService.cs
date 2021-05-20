using System.Linq;

using Serilog.Core;

using TheShop.Database;
using TheShop.Models;
using TheShop.Models.ViewModels;

namespace TheShop.Services
{
    public class ReportsService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IArticlesRepository articlesRepository;
        private readonly ISalesRepository salesRepository;
        private readonly Logger logger;

        public ReportsService(
            Logger logger,
            IOrdersRepository ordersRepository,
            ISalesRepository salesRepository)
        {
            this.logger = logger;
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