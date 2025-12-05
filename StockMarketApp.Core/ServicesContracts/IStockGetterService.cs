using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IStockGetterService
    {
        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'
        /// </summary>
        /// <returns>The list of buy orders from the table</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders(Guid userId);
        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'
        /// </summary>
        /// <returns>The list of sell order from the table</returns>
        Task<List<SellOrderResponse>> GetSellOrders(Guid userId);

    }
}
