using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
    public interface IStockRepository
    {

        /// <summary>
        /// Adds a new BuyOrder to the data store
        /// </summary>
        /// <param name="buyOrder">BuyOrder to add</param>
        /// <returns>Returns the BuyOrder after adding it to the data store</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Adds a new SellOrder to the data store
        /// </summary>
        /// <param name="sellOrder">SellOrder to add</param>
        /// <returns>Returns the SellOrder after adding it to the data store</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);


        /// <summary>
        /// Returns all BuyOrder from the data store
        /// </summary>
        /// <returns>List of all BuyOrder in the data store</returns>
        Task<List<BuyOrder>> GetBuyOrders(Guid userId);

        /// <summary>
        /// Returns all SellOrder from the data store
        /// </summary>
        /// <returns>List of all SellOrder in the data store</returns>
        Task<List<SellOrder>> GetSellOrders(Guid userId);
    }
}
