using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServicesContracts;
using Servicies;

namespace StockMarketApp.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration) 
        {

            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddScoped<IFinnhubGetterService, FinnhubGetterService>();
            services.AddScoped<IFinnhubSearcherService, FinnhubSearcherService>();
            services.AddScoped<IStockGetterService, StockGetterService>();
            services.AddScoped<IStockCreatorService, StockCreatorService>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging();
            });

            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

            return services;

        }
    }
}
