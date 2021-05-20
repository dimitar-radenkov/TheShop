using Moq;

using NUnit.Framework;

using Serilog;

using TheShop.Database;
using TheShop.Models;
using TheShop.Services;

namespace TheShop.Tests
{
    [TestFixture]
    public class OrdersServiceTests
    {
        private Mock<ILogger> loggerMock;
        private Mock<IOrdersRepository> ordersRepositoryMock;
        private Mock<IOffersRepository> offersRepositoryMock;
        private Mock<ISuppliersService> suppliersServiceMock;

        private OrdersService ordersService;

        [SetUp]
        public void Setup()
        {
            this.loggerMock = new Mock<ILogger>();
            this.ordersRepositoryMock = new Mock<IOrdersRepository>();
            this.offersRepositoryMock = new Mock<IOffersRepository>();
            this.suppliersServiceMock = new Mock<ISuppliersService>();

            this.ordersService = new OrdersService(
                loggerMock.Object,
                ordersRepositoryMock.Object,
                offersRepositoryMock.Object,
                suppliersServiceMock.Object);
        }

        [Test]
        public void GetOrder_WhenNotAcceptableOffers_ShouldNotAddToOfferRepo()
        {
            //arrange
            var orderId = 1;
            var articleId = 1;
            var maxPrice = 100;

            Order actualOrder = null;
            this.ordersRepositoryMock
                .Setup(x => x.Add(It.IsAny<Order>()))
                .Returns((Order o) =>
                {
                    actualOrder = o;
                    actualOrder.Id = orderId;

                    return actualOrder;
                });

            //act
            var result = ordersService.GetOrder(articleId, maxPrice);

            //assert
            Assert.IsFalse(result.HasValidOffer);
            Assert.IsNull(result.OfferId);
            Assert.AreEqual(actualOrder.Status, OrderStatus.Unfullfilled);

            this.offersRepositoryMock.Verify(x => x.Add(It.IsAny<Offer>()), Times.Never());
        }
    }
}