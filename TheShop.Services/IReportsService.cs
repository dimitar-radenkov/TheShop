using TheShop.Models.ViewModels;

namespace TheShop.Services
{
    public interface IReportsService
    {
        ArticleViewModel GetByArticleId(int articleId);
    }
}