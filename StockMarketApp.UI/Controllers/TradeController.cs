using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServicesContracts;
using ServicesContracts.DTO;
using StockMarketApp.Core.Domain.IdentityEntities;
using StockMarketApp.Filters.ActionFilters;
using StockMarketApp.Models;

namespace StockMarketApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubGetterService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStockCreatorService _stockCreatorService;
        private readonly IStockGetterService _stockGetterService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<TradeController> _logger;


        public TradeController(IFinnhubGetterService finnhubService, IOptions<TradingOptions> tradingOptions, 
                                IConfiguration configuration, IStockCreatorService stockCreatorService,
                                IStockGetterService stockGetterService, ILogger<TradeController> logger,
                                UserManager<ApplicationUser> userManager) 
        { 
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _stockCreatorService = stockCreatorService;
            _stockGetterService = stockGetterService;
            _userManager = userManager;

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

            if (!TryGetUserId(out Guid userId))
            {
                // User not logged in → return empty orders view
                return RedirectToAction("Orders");
            }

            await _stockCreatorService.CreateBuyOrder(orderRequest, userId);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]

        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            if (!TryGetUserId(out Guid userId))
            {
                return RedirectToAction("Orders");
            }

            await _stockCreatorService.CreateSellOrder(orderRequest, userId);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]

        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(Orders), nameof(TradeController));

            if (!TryGetUserId(out Guid userId))
            {
                return View(new Orders
                {
                    BuyOrders = new List<BuyOrderResponse>(),
                    SellOrders = new List<SellOrderResponse>()
                });
            }

            Orders orders = new Orders()
            {
                BuyOrders = await _stockGetterService.GetBuyOrders(userId),
                SellOrders = await _stockGetterService.GetSellOrders(userId)
            };

            return View(orders);
        }

        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogInformation("{MetodName} action method of {ControllerName}", nameof(OrdersPDF), nameof(TradeController));

            Orders orders = new Orders();

            var userId = Guid.Parse(_userManager.GetUserId(User));

            //Get list of sell and buy orders
            orders.SellOrders = await _stockGetterService.GetSellOrders(userId);
            orders.BuyOrders = await _stockGetterService.GetBuyOrders(userId);

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        private bool TryGetUserId(out Guid userId)
        {
            userId = default;

            var userIdString = _userManager.GetUserId(User);

            return Guid.TryParse(userIdString, out userId);
        }

    }
}
