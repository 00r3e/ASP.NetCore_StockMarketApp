using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServicesContracts;
using StockMarketApp.Models;

namespace StockMarketApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {

        private readonly IFinnhubGetterService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStockCreatorService _stockService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubGetterService finnhubService, IOptions<TradingOptions> tradingOptions,
            IConfiguration configuration, IStockCreatorService stockService, ILogger<StocksController> logger)
        {
            _configuration = configuration;
            _tradingOptions = tradingOptions.Value;
            _stockService = stockService;
            _finnhubService = finnhubService;
            _logger = logger;
        }

        [Route("[action]")]
        [Route("[action]/{stockSymbol}")]
        public async Task<IActionResult> Explore(string stockSymbol)
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(Explore), nameof(StocksController));

            ViewBag.CurrentStockSymbol = stockSymbol;

            // Get all stocks 
            List<Dictionary<string, string>>? allStocks = await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            List<string>? top25Stocks = _tradingOptions.Top25PopularStocks?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (stockSymbol != null)
            {
                var stockDetails = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

                ViewBag.StockDetails = (stockDetails != null) ? stockDetails.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.ToString() ?? string.Empty) : null;

                ViewBag.stockPrice = (stockPriceQuote != null) ? Convert.ToDouble(stockPriceQuote["c"].ToString()) : 0;
            }

            top25Stocks?.AddRange(allStocks.Where(s => top25Stocks.Equals(s["displaySymbol"])).Select(s => s["displaySymbol"]));

            foreach (string displaySimbol in top25Stocks)
            {
                stocks.Add(new Stock { StockSymbol = displaySimbol, StockName = allStocks.FirstOrDefault(s => s["displaySymbol"].Equals(displaySimbol))["description"] });
            }

            return View(stocks);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetStockDetails(string stockSymbol)
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(GetStockDetails), nameof(StocksController));

            ViewBag.CurrentStockSymbol = stockSymbol;

            var stockDetails = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            ViewBag.stockPrice = (stockPriceQuote != null) ? Convert.ToDouble(stockPriceQuote["c"].ToString()) : 0;

            var detailsDict = (stockDetails != null)
                ? stockDetails.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty)
                : null;

            return PartialView("_StockDetails", detailsDict);
        }
    }
}
