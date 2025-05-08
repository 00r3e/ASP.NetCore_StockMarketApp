using System.Collections.Generic;
using System.Diagnostics;
using ServicesContracts;
using ServicesContracts.DTO;
using Servicies;

namespace StockMarketAppTests
{
    public class StockServiceTest
    {
        //Arrange
        //Act
        //Assert
        private readonly IStockService _stockService;

        public StockServiceTest()
        {
            _stockService = new StockService();
        }

        #region CreateBuyOrder
        //When we supply null as BuyOrderRequest, it should throw ArgumentNullException
        [Fact]
        public void CreateBuyOrder_BuyOrderRequestIsNull()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When we supply BuyOrderRequest Quantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_InvalidQuantity()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 100
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When we supply BuyOrderRequest Price as 0 (as per the specification, minimum is 1), it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_InvalidPriceMinimum()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Price = 0,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When you supply BuyOrder Price as 100001 (as per the specification, maximum is 10000), it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_InvalidPriceMaximum()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Price = 100001,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = null,
                Price = 100,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        // it should be equal or newer date than 2000-01-01), it should throw ArgumentException
        [Fact]
        public void CreateBuyOrder_DateAndTimeOfOrderIsInvalid()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                DateAndTimeOfOrder = DateTime.Parse("1999-01-01"),
                Price = 100,
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateBuyOrder(buyOrderRequest)
            );
        }

        // When you supply all valid values, it should be successful and return an
        // object of BuyOrderResponse type with auto-generated BuyOrderID (guid)
        [Fact]
        public void CreateBuyOrder_ProperOrderDetails()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 100,
                Quantity = 1000
            };

            ///Act
            BuyOrderResponse buyOrderResponse = _stockService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.True(buyOrderResponse.BuyOrderID != Guid.Empty);

            Assert.Contains(buyOrderResponse, _stockService.GetBuyOrders());
        }

        #endregion


        #region CreateSellOrder
        //When we supply null as SellOrderRequest, it should throw ArgumentNullException
        [Fact]
        public void CreateSellOrder_SellOrderRequestIsNull()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When we supply SellOrderRequest Quantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_InvalidQuantity()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 100
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When we supply SellOrderRequest Price as 0 (as per the specification, minimum is 1), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_InvalidPriceMinimum()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Price = 0,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When we supply SellOrder Price as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_InvalidPriceMaximum()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                Price = 1000100,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        // When we supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_StockSymbolIsNull()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = null,
                Price = 100,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        // When we supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        // it should be equal or newer date than 2000-01-01), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_DateAndTimeOfOrderIsInvalid()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                DateAndTimeOfOrder = DateTime.Parse("1999-01-01"),
                Price = 100,
                Quantity = 1000
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
                //Act
                _stockService.CreateSellOrder(sellOrderRequest)
            );
        }

        // When we supply all valid values, it should be successful and return an
        // object of SellOrderResponse type with auto-generated SellOrderID (guid)
        [Fact]
        public void CreateSellOrder_ProperOrderDetails()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = "sample Name",
                StockSymbol = "sample Symbol",
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 100,
                Quantity = 1000
            };

            ///Act
            SellOrderResponse sellOrderResponse = _stockService.CreateSellOrder(sellOrderRequest);

            //Assert
            Assert.True(sellOrderResponse.SellOrderID != Guid.Empty);

            Assert.Contains(sellOrderResponse, _stockService.GetSellOrders());
        }

        #endregion

        #region GetBuyOrders
        //When we invoke this method, by default, the returned list should be empty
        [Fact]
        public void GetBuyOrders_EmptyList()
        {
            Assert.True(_stockService.GetBuyOrders().Count == 0);
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method;
        //the returned list should contain all the same sell orders
        [Fact]
        public void GetBuyOrders_AddingProperValues()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
            {
                StockName = "sample name 1",
                StockSymbol = "sample symbol 1",
                Quantity= 1,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };
            BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
            {
                StockName = "sample name 2",
                StockSymbol = "sample symbol 2",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };
            BuyOrderRequest buyOrderRequest3 = new BuyOrderRequest()
            {
                StockName = "sample name 3",
                StockSymbol = "sample symbol 3",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };

            //Act
            BuyOrderResponse buyOrderResponse1 = _stockService.CreateBuyOrder(buyOrderRequest1);
            BuyOrderResponse buyOrderResponse2 = _stockService.CreateBuyOrder(buyOrderRequest2);
            BuyOrderResponse buyOrderResponse3 = _stockService.CreateBuyOrder(buyOrderRequest3);

            List<BuyOrderResponse> buyOrderResponses = _stockService.GetBuyOrders();

            //Assert
            Assert.Contains(buyOrderResponse1, buyOrderResponses);
            Assert.Contains(buyOrderResponse2, buyOrderResponses);
            Assert.Contains(buyOrderResponse3, buyOrderResponses);

        }

        #endregion

        #region GetSellOrders
        //When we invoke this method, by default, the returned list should be empty
        [Fact]
        public void GetSellOrders_EmptyList()
        {
            Assert.True(_stockService.GetSellOrders().Count == 0);
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method;
        //the returned list should contain all the same sell orders
        [Fact]
        public void GetSellrders_AddingProperValues()
        {
            //Arrange
            SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
            {
                StockName = "sample name 1",
                StockSymbol = "sample symbol 1",
                Quantity = 1,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };
            SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
            {
                StockName = "sample name 2",
                StockSymbol = "sample symbol 2",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };
            SellOrderRequest sellOrderRequest3 = new SellOrderRequest()
            {
                StockName = "sample name 3",
                StockSymbol = "sample symbol 3",
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.Parse("2011-01-01"),
                Price = 2
            };

            //Act
            SellOrderResponse sellOrderResponse1 = _stockService.CreateSellOrder(sellOrderRequest1);
            SellOrderResponse sellOrderResponse2 = _stockService.CreateSellOrder(sellOrderRequest2);
            SellOrderResponse sellOrderResponse3 = _stockService.CreateSellOrder(sellOrderRequest3);

            List<SellOrderResponse> sellOrderResponses = _stockService.GetSellOrders();

            //Assert
            Assert.Contains(sellOrderResponse1, sellOrderResponses);
            Assert.Contains(sellOrderResponse2, sellOrderResponses);
            Assert.Contains(sellOrderResponse3, sellOrderResponses);

        }

        #endregion
    }
}