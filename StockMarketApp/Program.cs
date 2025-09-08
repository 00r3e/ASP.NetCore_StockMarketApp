using System.Runtime;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServicesContracts;
using Servicies;
using StockMarketApp;
using StockMarketApp.Middleware;
using StockMarketApp.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration) // Read configuration settings from build-in IConfiguration
        .ReadFrom.Services(service); // Read app's services 
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
