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
    public class StockGetterService : IStockGetterService
    {
        //private field
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<StockGetterService> _logger;

        public StockGetterService(IStockRepository stockRepository, ILogger<StockGetterService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

       
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetBuyOrders), nameof(StockGetterService));

            var buyOrders = await _stockRepository.GetBuyOrders();
            return buyOrders.Select(bo => bo.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetSellOrders), nameof(StockGetterService));

            var sellOrders = await _stockRepository.GetSellOrders();
            return sellOrders.Select(bo => bo.ToSellOrderResponse()).ToList();
        }
    }
}
