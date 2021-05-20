using Autofac;

using Serilog;

using TheShop.Database;
using TheShop.Services;
using TheShop.Utils;

namespace TheShop
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            var logger = new LoggerConfiguration()
                .Enrich.With(new ThreadIdEnricher(), new CallerEnricher())
                .MinimumLevel.Verbose()
                .WriteTo.File(
                    path: "TheShopLog.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] ({ThreadId}) {Caller} {Message}{NewLine}{Exception}",
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 20_000_000)
                .CreateLogger();

            //other
            builder.RegisterInstance(logger);
            builder.RegisterType<Application>().As<IApplication>();

            //repositories
            builder.RegisterType<OrdersRepository>().As<IOrdersRepository>();
            builder.RegisterType<ArticlesRepository>().As<IArticlesRepository>();
            builder.RegisterType<SalesRepository>().As<ISalesRepository>();
            builder.RegisterType<OffersRepository>().As<IOffersRepository>();

            //providers
            builder.RegisterType<SuppliersProvider>().As<ISuppliersProvider>();

            //services
            builder.RegisterType<SuppliersService>().As<ISuppliersService>();
            builder.RegisterType<ReportsService>().As<IReportsService>();
            builder.RegisterType<SalesService>().As<ISalesService>();
            builder.RegisterType<OrdersService>().As<IOrdersService>();

            return builder.Build();
        }
    }
}