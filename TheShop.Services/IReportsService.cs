using TheShop.Models.ViewModels;

namespace TheShop.Services
{
    public interface IReportsService
    {
        ReportViewModel GetByArticleId(int articleId);
    }
}