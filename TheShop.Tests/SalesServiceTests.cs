using System;

using Moq;

using NUnit.Framework;

using Serilog;

using TheShop.Database;
using TheShop.Models.Entities;
using TheShop.Services;

namespace TheShop.Tests
{
    [TestFixture]
    public class SalesServiceTests
    {
        private Mock<ILogger> loggerMock;
        private Mock<IOrdersRepository> ordersRepositoryMock;
        private Mock<ISalesRepository> salesReposotiryMock;

        private SalesService salesService;

        [SetUp]
        public void Setup()
        {
            this.loggerMock = new Mock<ILogger>();
            this.ordersRepositoryMock = new Mock<IOrdersRepository>();
            this.salesReposotiryMock = new Mock<ISalesRepository>();

            this.salesService = new SalesService(
                loggerMock.Object,
                ordersRepositoryMock.Object,
                salesReposotiryMock.Object);
        }

        [Test]
        public void Sell_WhenNoException_ShouldAddSaleToRepo()
        {
            //arrange
            var orderId = 1;
            var offerId = 1;
            var buyerId = 1;

            var order = new Order { Id = orderId, ArticleId = 1, Status = OrderStatus.Fulfilled };

            this.ordersRepositoryMock
                .Setup(x => x.Get(orderId))
                .Returns(order);

            //act
            salesService.Sell(orderId, offerId, buyerId);

            //assert
            Assert.AreEqual(order.Status, OrderStatus.Completed);
            this.salesReposotiryMock
                .Verify(x => x.Add(It.Is<Sale>(s => 
                    s.BuyerId == buyerId && 
                    s.OfferId == offerId && 
                    s.OrderId == orderId)), Times.Once());
        }

        [Test]
        public void Sell_WhenException_ShouldRethrowServiceException()
        {
            //arrange
            this.ordersRepositoryMock
                .Setup(x => x.Get(1))
                .Throws(new Exception());

            //act && assert
            Assert.Throws<ServiceException>(() => salesService.Sell(orderId: 1, offerId: 1, buyerId: 1));
        }
    }
}