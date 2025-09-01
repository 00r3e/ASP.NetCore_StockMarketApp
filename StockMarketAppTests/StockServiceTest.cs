using System.Collections.Generic;
using System.Diagnostics;
using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
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
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly IStockRepository _stockRepository;

        private readonly IFixture _fixture;

        public StockServiceTest()
        {
            _fixture = new Fixture();

            _stockRepositoryMock = new Mock<IStockRepository>();
            _stockRepository = _stockRepositoryMock.Object;

            _stockService = new StockService(_stockRepository);
        }

        #region CreateBuyOrder
        //When we supply null as BuyOrderRequest, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_BuyOrderRequestNull_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act

            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        //When we supply BuyOrderRequest Quantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_InvalidQuantity_ToBeArgumentException()
        {

            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001).With(temp => temp.Price, (uint)100).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply BuyOrderRequest Price as 0 (as per the specification, minimum is 1), it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_InvalidPriceMinimum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)0).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply BuyOrder Price as 100001 (as per the specification, maximum is 10000), it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_InvalidPriceMaximum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)100001).Create();

            //Assert
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            //Act
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)100)
                .With(temp => temp.StockSymbol, null as string).Create();

            //Assert
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            //Act
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        // it should be equal or newer date than 2000-01-01), it should throw ArgumentException
        [Fact]
        public async Task CreateBuyOrder_DateAndTimeOfOrderIsInvalid_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-01-01"))
                .Create();
            //Assert
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRequest);
            };

            //Act
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // When you supply all valid values, it should be successful and return an
        // object of BuyOrderResponse type with auto-generated BuyOrderID (guid)
        [Fact]
        public async Task CreateBuyOrder_ProperOrderDetails_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            _stockRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            ///Act
            BuyOrderResponse buyOrderResponseFromCreate = await _stockService.CreateBuyOrder(buyOrderRequest);

            //Assert
            buyOrderResponseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreate.Should().Be(buyOrderResponseFromCreate);

        }

        #endregion


        #region CreateSellOrder
        //When we supply null as SellOrderRequest, it should throw ArgumentNullException
        [Fact]
        public async Task CreateSellOrder_SellOrderRequestIsNull_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply SellOrderRequest Quantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_InvalidQuantity_ToBeArgumentException()
        {

            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001).With(temp => temp.Price, (uint)100).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply SellOrderRequest Price as 0 (as per the specification, minimum is 1), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_InvalidPriceMinimum_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)0).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply SellOrder Price as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_InvalidPriceMaximum_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)1000100).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When we supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)100)
                .With(temp => temp.StockSymbol, null as string).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When we supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        // it should be equal or newer date than 2000-01-01), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_DateAndTimeOfOrderIsInvalid_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)1000).With(temp => temp.Price, (uint)100)
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-01-01")).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When we supply all valid values, it should be successful and return an
        // object of SellOrderResponse type with auto-generated SellOrderID (guid)
        [Fact]
        public async Task CreateSellOrder_ProperOrderDetails_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

            _stockRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            ///Act
            SellOrderResponse sellOrderResponseFromCreate = await _stockService.CreateSellOrder(sellOrderRequest);

            //Assert
            sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponseFromCreate);
        }

        #endregion

        #region GetBuyOrders
        //When we invoke this method, by default, the returned list should be empty
        [Fact]
        public async Task GetBuyOrders_EmptyList_ToBeEmpty()
        {
            //Arrange
            _stockRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());
            //Act
            var buyOrders = await _stockService.GetBuyOrders();
            //Assert

            buyOrders.Should().BeEmpty();
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method;
        //the returned list should contain all the same sell orders
        [Fact]
        public async Task GetBuyOrders_AddingProperValues_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>
            {
                _fixture.Build<BuyOrder>().Create(),
                _fixture.Build<BuyOrder>().Create(),
                _fixture.Build<BuyOrder>().Create()
            };

            List<BuyOrderResponse> buyOrderResponseListExpected = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            //Act
            _stockRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);
            List<BuyOrderResponse> buyOrderResponseListFromGet = await _stockService.GetBuyOrders();

            //Assert
            buyOrderResponseListFromGet.Should().BeEquivalentTo(buyOrderResponseListExpected);

        }

        #endregion

        #region GetSellOrders
        //When we invoke this method, by default, the returned list should be empty
        [Fact]
        public async Task GetSellOrders_EmptyList_ToBeEmpty()
        {
            //Arrange
            _stockRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());
            
            //Act
            var sellOrders = await _stockService.GetSellOrders();
            
            //Assert
            sellOrders.Should().BeEmpty();
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method;
        //the returned list should contain all the same sell orders
        [Fact]
        public async Task GetSellrders_AddingProperValues_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>
            {
                _fixture.Build<SellOrder>().Create(),
                _fixture.Build<SellOrder>().Create(),
                _fixture.Build<SellOrder>().Create()
            };

            List<SellOrderResponse> sellOrderResponseListExpected = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            //Act
            _stockRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);
            List<SellOrderResponse> sellOrderResponseListFromGet = await _stockService.GetSellOrders();

            //Assert
            sellOrderResponseListFromGet.Should().BeEquivalentTo(sellOrderResponseListExpected);
        }

        #endregion
    }
}