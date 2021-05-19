
using Autofac;

using TheShop.Database;
using TheShop.Services;

namespace TheShop
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<OrdersRepository>().As<IOrdersRepository>();
            builder.RegisterType<ArticlesRepository>().As<IArticlesRepository>();
            builder.RegisterType<SalesRepository>().As<ISalesRepository>();
            builder.RegisterType<OffersRepository>().As<IOffersRepository>();
            builder.RegisterType<SuppliersProvider>().As<ISuppliersProvider>();
            builder.RegisterType<SuppliersService>().As<ISuppliersService>();
            builder.RegisterType<ShopService>();

            return builder.Build();
        }
    }
}