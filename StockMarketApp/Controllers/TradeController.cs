using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Servicies;

namespace StockMarketApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration) 
        { 
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
        }

        [Route("/")]
        [Route("/Trade/Index")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }

            // Get stock price quote data from the Finnhub service
            Dictionary<string, object>? finnhubStockPriceQuote = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            // Get company profile data from the Finnhub service
            Dictionary<string, object>? finnhubCompanyProfile = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);
            // Store the Finnhub token in ViewBag
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            //Create stock trade object
            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
                
                StockName = (finnhubCompanyProfile != null) ? finnhubCompanyProfile["name"].ToString() : null,
                Price = (finnhubStockPriceQuote != null) ? Convert.ToDouble(finnhubStockPriceQuote["c"].ToString()) : 0
            };
            // Pass the stockTrade object to the view
            return View(stockTrade);
        }
    }
}
