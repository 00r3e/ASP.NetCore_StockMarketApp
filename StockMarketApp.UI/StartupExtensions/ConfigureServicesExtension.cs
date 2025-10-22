using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServicesContracts;
using Servicies;
using StockMarketApp.Core.Domain.IdentityEntities;

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

            //Enable Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })

                .AddEntityFrameworkStores<OrdersDbContext>()

                .AddDefaultTokenProviders()

                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, OrdersDbContext, Guid>>()

                .AddRoleStore<RoleStore<ApplicationRole, OrdersDbContext, Guid>>();


            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

            return services;

        }
    }
}
