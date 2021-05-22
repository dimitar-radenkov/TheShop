
using Autofac;

using TheShop.Database;

namespace TheShop
{
    internal class Program
    {
        private static void Main()
        {
            var container = ContainerConfig.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var articles = scope.Resolve<IArticlesRepository>();
                //pre seed articles
                articles.Add(new Models.Entities.Article { Id = 1, Name = "Item1" });
                articles.Add(new Models.Entities.Article { Id = 2, Name = "Item2" });
                articles.Add(new Models.Entities.Article { Id = 3, Name = "Item3" });
                articles.Add(new Models.Entities.Article { Id = 4, Name = "Item4" });
                articles.Add(new Models.Entities.Article { Id = 5, Name = "Item5" });

                var app = scope.Resolve<IApplication>();
                app.Run();
            }
        }
    }
}