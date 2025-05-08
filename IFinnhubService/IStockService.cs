using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IStockService
    {
        /// <summary>
        ///  Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrderRequest"> the order request to add in the table</param>
        /// <returns>the buy order response from the buy order that added in the table</returns>
        BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        /// <summary>
        /// Inserts a new sell order into the database table called 'SellOrders'
        /// </summary>
        /// <param name="sellOrderRequest"> the sell request to add in the table</param>
        /// <returns>sell order response from the sell order that added in the table</returns>
        SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest);
        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'
        /// </summary>
        /// <returns>The list of buy orders from the table</returns>
        List<BuyOrderResponse> GetBuyOrders();
        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'
        /// </summary>
        /// <returns>The list of sell order from the table</returns>
        List<SellOrderResponse> GetSellOrders();

    }
}
