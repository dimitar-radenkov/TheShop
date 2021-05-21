using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using Serilog;

using TheShop.Database;
using TheShop.Models;
using TheShop.Models.Entities;
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
                this.loggerMock.Object,
                this.ordersRepositoryMock.Object,
                this.offersRepositoryMock.Object,
                this.suppliersServiceMock.Object);
        }

        [Test]
        public void GetOrder_WhenNoAcceptableOffers_ShouldReturnInvalidOffer()
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

        [Test]
        public void GetOrder_WhenNoOffers_ShouldReturnInvalidOffer()
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

            this.suppliersServiceMock
                .Setup(x => x.GetArticles(articleId))
                .Returns(Enumerable.Empty<ArticleWithPrice>());

            //act
            var result = ordersService.GetOrder(articleId, maxPrice);

            //assert
            Assert.IsFalse(result.HasValidOffer);
            Assert.IsNull(result.OfferId);
            Assert.AreEqual(actualOrder.Status, OrderStatus.Unfullfilled);

            this.offersRepositoryMock.Verify(x => x.Add(It.IsAny<Offer>()), Times.Never());
        }

        [Test]
        public void GetOrder_ExceptionIsThrown_ShouldRethrowServiceException()
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

            this.suppliersServiceMock
                .Setup(x => x.GetArticles(articleId))
                .Throws(new Exception());

            //act && assert
            Assert.Throws<ServiceException>(() => ordersService.GetOrder(articleId, maxPrice));
        }

        [Test]
        public void GetOrder_WhenAcceptableOffers_ShouldReturnBestOne()
        {
            //arrange
            var orderId = 1;
            var articleId = 1;
            var maxPrice = 49;

            Order actualOrder = null;
            this.ordersRepositoryMock
                .Setup(x => x.Add(It.IsAny<Order>()))
                .Returns<Order>(o =>
                {
                    actualOrder = o;
                    actualOrder.Id = orderId;

                    return actualOrder;
                });

            var articles = new List<ArticleWithPrice>()
            {
                new ArticleWithPrice
                {
                    Id = articleId,
                    SupplierId = 1,
                    Name = "Item",
                    Price = 100,
                },
                new ArticleWithPrice
                {
                    Id = articleId,
                    SupplierId = 2,
                    Name = "Item",
                    Price = 25,
                },
                new ArticleWithPrice
                {
                    Id = articleId,
                    SupplierId = 3,
                    Name = "Item",
                    Price = 50,
                },
                new ArticleWithPrice
                {
                    Id = articleId,
                    SupplierId = 4,
                    Name = "Item",
                    Price = 1,
                },
            };

            this.suppliersServiceMock
                .Setup(x => x.GetArticles(articleId))
                .Returns(articles);

            var offerId = 1;
            var offersList = new List<Offer>();
            this.offersRepositoryMock
                .Setup(x => x.Add(It.IsAny<Offer>()))
                .Returns<Offer>(offer => 
                {
                    offer.Id = offerId++;
                    return offer;
                })
                .Callback<Offer>(order => offersList.Add(order));

            //act
            var result = ordersService.GetOrder(articleId, maxPrice);

            //assert
            Assert.IsTrue(result.HasValidOffer);
            Assert.IsNotNull(result.OfferId);
            Assert.AreEqual(actualOrder.Status, OrderStatus.Fulfilled);

            var expectedCallCount = articles.Where(x => x.Price <= maxPrice).Count();
            this.offersRepositoryMock.Verify(x => x.Add(It.IsAny<Offer>()), Times.Exactly(expectedCallCount));

            var expectedOfferId = 1;
            Assert.AreEqual(expectedOfferId, result.OfferId);
        }
    }
}