using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServicesContracts;
using ServicesContracts.DTO;
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

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, 
                                IConfiguration configuration, IStockService stockService) 
        { 
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _stockService = stockService;
        }

        [Route("/")]
        [Route("[action]")]
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
                Price = (finnhubStockPriceQuote != null) ? Convert.ToDouble(finnhubStockPriceQuote["c"].ToString()) : 0,
                Quantity = Convert.ToUInt32(_tradingOptions.DefaultOrderQuantity)
            };
            
            // Pass the stockTrade object to the view
            return View(stockTrade);
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return RedirectToAction("Index", "Trade");
            }

            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return RedirectToAction("Index", "Trade");
            }

            SellOrderResponse sellOrderResponse = await _stockService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]

        public async Task<IActionResult> Orders()
        {
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
