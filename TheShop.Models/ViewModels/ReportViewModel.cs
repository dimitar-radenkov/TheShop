using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheShop.Models.ViewModels
{
    public class SaleViewModel
    {
        public decimal Price { get; set; }
        public string Buyer { get; set; }
        public DateTime DateSold { get; set; }
        public DateTime DateOrdered { get; set; }

        public override string ToString()
        {
            return $"Sold for {this.Price} on {this.DateSold} by '{this.Buyer}' ordered on {this.DateOrdered}";
        }
    }

    public class ReportViewModel
    {
        public int Id { get; }
        public string Name { get; }
        public IEnumerable<SaleViewModel> Sales { get; }

        public ReportViewModel(int id, string name, IEnumerable<SaleViewModel> sales)
        {
            this.Id = id;
            this.Name = name;
            this.Sales = sales;
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
                    sb.AppendLine($"\t{sale}");
                }
            }

            return sb.ToString();
        }
    }
}
