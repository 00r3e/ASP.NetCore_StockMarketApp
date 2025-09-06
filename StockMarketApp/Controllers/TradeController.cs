using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServicesContracts;
using ServicesContracts.DTO;
using StockMarketApp.Filters.ActionFilters;
using StockMarketApp.Models;

namespace StockMarketApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStockService _stockService;
        private readonly ILogger<TradeController> _logger;


        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, 
                                IConfiguration configuration, IStockService stockService, ILogger<TradeController> logger) 
        { 
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _stockService = stockService;
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("[action]/{stockSymbol}")]

        public async Task<IActionResult> Index(string stockSymbol)
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}",  nameof(Index), nameof(TradeController));


            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }
            if (stockSymbol == null) 
            {
                stockSymbol = _tradingOptions.DefaultStockSymbol;
            }

            if (_tradingOptions.DefaultOrderQuantity == null)
            {
                _tradingOptions.DefaultOrderQuantity = 100;
            }

            // Get stock price quote data from the Finnhub service
            Dictionary<string, object>? finnhubStockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            // Get company profile data from the Finnhub service
            Dictionary<string, object>? finnhubCompanyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            // Store the Finnhub token in ViewBag
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];


            //Create stock trade object
            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = stockSymbol,

                StockName = (finnhubCompanyProfile != null) ? finnhubCompanyProfile["name"].ToString() : null,
                Price = (finnhubStockPriceQuote != null) ? Convert.ToDouble(finnhubStockPriceQuote["c"].ToString()) : 0,
                Quantity = Convert.ToUInt32(_tradingOptions.DefaultOrderQuantity)
            };

            // Pass the stockTrade object to the view
            return View(stockTrade);
        }


        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(BuyOrder), nameof(TradeController));

            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(orderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]

        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}",  nameof(SellOrder), nameof(TradeController));

            SellOrderResponse sellOrderResponse = await _stockService.CreateSellOrder(orderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]

        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(Orders), nameof(TradeController));

            Orders orders = new Orders()
            {
                BuyOrders = await _stockService.GetBuyOrders(),
                SellOrders = await _stockService.GetSellOrders()
            };

            return View(orders);
        }

        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(OrdersPDF), nameof(TradeController));

            Orders orders = new Orders();

            //Get list of sell and buy orders
            orders.SellOrders = await _stockService.GetSellOrders();
            orders.BuyOrders = await _stockService.GetBuyOrders();

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

    }
}
