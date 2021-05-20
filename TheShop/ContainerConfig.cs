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

            builder.RegisterInstance(logger);
            builder.RegisterType<OrdersRepository>().As<IOrdersRepository>();
            builder.RegisterType<ArticlesRepository>().As<IArticlesRepository>();
            builder.RegisterType<SalesRepository>().As<ISalesRepository>();
            builder.RegisterType<OffersRepository>().As<IOffersRepository>();
            builder.RegisterType<SuppliersProvider>().As<ISuppliersProvider>();
            builder.RegisterType<SuppliersService>().As<ISuppliersService>();
            builder.RegisterType<ReportsService>();

            return builder.Build();
        }
    }
}