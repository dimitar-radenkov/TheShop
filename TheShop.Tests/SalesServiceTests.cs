using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using Serilog;

using TheShop.Database;
using TheShop.Models.Entities;
using TheShop.Services;

namespace TheShop.Tests
{
    [TestFixture]
    public class ReportsServiceTests
    {
        private Mock<ILogger> loggerMock;
        private Mock<IOrdersRepository> ordersRepositoryMock;
        private Mock<ISalesRepository> salesReposotiryMock;
        private Mock<IArticlesRepository> articlesRepositoryMock;
        private Mock<IOffersRepository> offersRepositoryMock;

        private ReportsService reportsService;

        [SetUp]
        public void Setup()
        {
            this.loggerMock = new Mock<ILogger>();
            this.ordersRepositoryMock = new Mock<IOrdersRepository>();
            this.salesReposotiryMock = new Mock<ISalesRepository>();
            this.articlesRepositoryMock = new Mock<IArticlesRepository>();
            this.offersRepositoryMock = new Mock<IOffersRepository>();

            this.reportsService = new ReportsService(
                loggerMock.Object,
                this.articlesRepositoryMock.Object,
                this.ordersRepositoryMock.Object,
                this.salesReposotiryMock.Object,
                this.offersRepositoryMock.Object);
        }

        [Test]
        public void GetByArticleId_ShouldShowCorrectReport()
        {
            //arrange
            var article = new Article { Id = 1, Name = "Item" };
            this.articlesRepositoryMock
                .Setup(x => x.Get(article.Id))
                .Returns(article);

            var order1 = new Order { ArticleId = 1, Id = 1, Status = OrderStatus.Completed, DateCreated = DateTime.UtcNow.Subtract(TimeSpan.FromDays(3)) };
            var order2 = new Order { ArticleId = 1, Id = 2, Status = OrderStatus.Completed, DateCreated = DateTime.UtcNow.Subtract(TimeSpan.FromDays(2)) };
            this.ordersRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Order> { order1, order2 });

            var sale1 = new Sale { Id = 1, BuyerId = 1, OfferId = 1, OrderId = order1.Id, DateSold = DateTime.UtcNow };
            var sale2 = new Sale { Id = 2, BuyerId = 2, OfferId = 2, OrderId = order2.Id, DateSold = DateTime.UtcNow };
            this.salesReposotiryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Sale> { sale1, sale2});

            var offer1 = new Offer { Id = 1, ArticleId = article.Id, OrderId = order1.Id, Price = 20, SupplierId = 1 };
            var offer2 = new Offer { Id = 2, ArticleId = article.Id, OrderId = order2.Id, Price = 30, SupplierId = 1 };
            this.offersRepositoryMock
                .Setup(x => x.Get(sale1.OfferId))
                .Returns(offer1);

            this.offersRepositoryMock
                .Setup(x => x.Get(sale2.OfferId))
                .Returns(offer2);

            //act
            var report = this.reportsService.GetByArticleId(1);
            var sales = report.Sales.ToList();

            //assert
            Assert.AreEqual(report.Id, article.Id);
            Assert.AreEqual(report.Name, article.Name);

            Assert.AreEqual(2, sales.Count);
            Assert.AreEqual(offer1.Price, sales[0].Price);
            Assert.AreEqual(offer2.Price, sales[1].Price);
        }
    }



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