using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Servicies.Helpers;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Servicies
{
    public class StockCreatorService : IStockCreatorService
    {
        //private field
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<StockCreatorService> _logger;

        public StockCreatorService(IStockRepository stockRepository, ILogger<StockCreatorService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest, Guid userId)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(CreateBuyOrder), nameof(StockCreatorService));

            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
             
            buyOrder.BuyOrderID = Guid.NewGuid();
            buyOrder.UserId = userId;

            await _stockRepository.CreateBuyOrder(buyOrder);

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            return buyOrderResponse;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest, Guid userId)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(CreateSellOrder), nameof(StockCreatorService));

            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();
            sellOrder.UserId = userId;

            await _stockRepository.CreateSellOrder(sellOrder);

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            return sellOrderResponse;
        }

    }
}
