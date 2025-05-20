using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServicesContracts;
using ServicesContracts.DTO;
using Servicies;
using StockMarketApp.Models;

namespace StockMarketApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStockService _stockService;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, 
                                IConfiguration configuration, IStockService stockService) 
        { 
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _stockService = stockService;
        }

        [Route("/")]
        [Route("/Trade/Index")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }

            if (_tradingOptions.DefaultOrderQuantity == null)
            {
                _tradingOptions.DefaultOrderQuantity = 100;
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


        [Route("/Trade/BuyOrder")]
        [HttpPost]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return RedirectToAction("Index", "Trade");
            }

            BuyOrderResponse buyOrderResponse = _stockService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("/Trade/SellOrder")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return RedirectToAction("Index", "Trade");
            }

            SellOrderResponse sellOrderResponse = _stockService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("/Trade/Orders")]
        [HttpGet]

        public IActionResult Orders()
        {
            Orders orders = new Orders()
            {
                BuyOrders = _stockService.GetBuyOrders(),
                SellOrders = _stockService.GetSellOrders()
            };

            return View(orders);
        }
    }
}
