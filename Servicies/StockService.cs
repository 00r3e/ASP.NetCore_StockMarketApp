using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts;
using ServicesContracts.DTO;
using Entities;
using Servicies.Helpers;

namespace Servicies
{
    public class StockService : IStockService
    {
        //private field
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StockService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
             
            buyOrder.BuyOrderID = Guid.NewGuid();

            _buyOrders.Add(buyOrder);

            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            return buyOrderResponse;
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if(sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model Validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            _sellOrders.Add(sellOrder);

            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();

            return sellOrderResponse;
        }

       
        public List<BuyOrderResponse> GetBuyOrders()
        {
            return _buyOrders.Select(bo => bo.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrders.Select(so => so.ToSellOrderResponse()).ToList();
        }
    }
}
