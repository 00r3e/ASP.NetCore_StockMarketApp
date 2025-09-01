using System.Runtime;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServicesContracts;
using Servicies;
using StockMarketApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging(); 
});

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
