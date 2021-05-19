using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheShop.Models.ViewModels
{
    public class SaleViewModel
    {
        
    }

    public class ArticleViewModel
    {
        public int Id { get; }
        public string Name { get; }
        public IEnumerable<SaleViewModel> Sales { get; }

        public ArticleViewModel(int id, string name)
        {

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Article: {this.Name} [{this.Id}]");

            if (this.Sales.Any())
            {
                sb.AppendLine("\tSales:");
                foreach (var sale in this.Sales)
                {
                    sb.AppendLine($"");
                }
            }

            return sb.ToString();
        }
    }
}
