using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IStockCreatorService
    {
        /// <summary>
        ///  Inserts a new buy order into the database table called 'BuyOrders'.
        /// </summary>
        /// <param name="buyOrderRequest"> the order request to add in the table</param>
        /// <returns>the buy order response from the buy order that added in the table</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        /// <summary>
        /// Inserts a new sell order into the database table called 'SellOrders'
        /// </summary>
        /// <param name="sellOrderRequest"> the sell request to add in the table</param>
        /// <returns>sell order response from the sell order that added in the table</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
    }
}
