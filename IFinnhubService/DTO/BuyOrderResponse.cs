using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServicesContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;

            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return (BuyOrderID == buyOrderResponse.BuyOrderID && StockSymbol == buyOrderResponse.StockSymbol &&
                StockName == buyOrderResponse.StockName && DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder) &&
                Quantity == buyOrderResponse.Quantity && Price == buyOrderResponse.Price && TradeAmount == buyOrderResponse.TradeAmount;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID},Stock Symbol: {StockSymbol},Stock Name: {StockName},Date And Time Of Order: " +
            $"{DateAndTimeOfOrder},Quantity: {Quantity.ToString()},Price: {Price.ToString()},Trade Amound: {TradeAmount.ToString()}";
        }

        
    }

    public static class BuyOrderExtensions
    {
        /// <summary>
        /// An extension method to convert an object of BuyOrder class into BuyOrderResponse class
        /// </summary>
        /// <param name="buyOrder">The BuyOrder object to convert</param>
        /// <returns>Returns the converted BuyOrderResponse object</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
