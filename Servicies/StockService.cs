using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Servicies.Helpers;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Servicies
{
    public class StockService : IStockService
    {
        //private field
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(IStockRepository stockRepository, ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(CreateBuyOrder), nameof(StockService));

            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
             
            buyOrder.BuyOrderID = Guid.NewGuid();

            await _stockRepository.CreateBuyOrder(buyOrder);

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            return buyOrderResponse;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(CreateSellOrder), nameof(StockService));

            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            await _stockRepository.CreateSellOrder(sellOrder);

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            return sellOrderResponse;
        }

       
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetBuyOrders), nameof(StockService));

            var buyOrders = await _stockRepository.GetBuyOrders();
            return buyOrders.Select(bo => bo.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetSellOrders), nameof(StockService));

            var sellOrders = await _stockRepository.GetSellOrders();
            return sellOrders.Select(bo => bo.ToSellOrderResponse()).ToList();
        }
    }
}
