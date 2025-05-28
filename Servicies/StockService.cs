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

namespace Servicies
{
    public class StockService : IStockService
    {
        //private field
        private readonly OrdersDbContext _ordersDbContext;
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StockService(OrdersDbContext ordersDbContext)
        {
            _ordersDbContext = ordersDbContext;
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
             
            buyOrder.BuyOrderID = Guid.NewGuid();

            await _ordersDbContext.BuyOrders.AddAsync(buyOrder);
            await _ordersDbContext.SaveChangesAsync();

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            return buyOrderResponse;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if(sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            await _ordersDbContext.SellOrders.AddAsync(sellOrder);
            await _ordersDbContext.SaveChangesAsync();

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            return sellOrderResponse;
        }

       
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return await _ordersDbContext.BuyOrders.Select(bo => bo.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return await _ordersDbContext.SellOrders.Select(so => so.ToSellOrderResponse()).ToListAsync();
        }
    }
}
