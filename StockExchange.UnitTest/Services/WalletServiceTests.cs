using FluentAssertions;
using Moq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class WalletServiceTests
    {
        private const int userId = 1;

        private readonly IWalletService _service;
        private readonly Mock<IRepository<UserTransaction>> _transactionsRepository = new Mock<IRepository<UserTransaction>>();
        private readonly Mock<IPriceService> _priceService = new Mock<IPriceService>();

        public WalletServiceTests()
        {
            _service = new WalletService(_transactionsRepository.Object, _priceService.Object);
        }

        [Fact]
        public void Should_return_only_companies_which_stocks_user_currently_owns()
        {
            SetupTransactions(
                new UserTransaction { CompanyId = 1, Price = 10, Quantity = 1,  UserId = userId },
                new UserTransaction { CompanyId = 1, Price = 20, Quantity = -1, UserId = userId },
                new UserTransaction { CompanyId = 2, Price = 5,  Quantity = 2,  UserId = userId }
           );
            SetupCurrentPrices(new Price { CompanyId = 2, ClosePrice = 10 });

            var results = _service.GetOwnedStocks(userId);

            results.Count.Should().Be(1);
            results[0].CompanyId.Should().Be(2);
        }

        [Fact]
        public void Should_set_current_prices_to_correct_values()
        {
            SetupTransactions(
                new UserTransaction { CompanyId = 1, Price = 10, Quantity = 1, UserId = userId },
                new UserTransaction { CompanyId = 2, Price = 10, Quantity = 2, UserId = userId }
            );
            SetupCurrentPrices(
                new Price { CompanyId = 1, ClosePrice = 10 },
                new Price { CompanyId = 2, ClosePrice = 40 }
            );

            var results = _service.GetOwnedStocks(userId);

            results.Count.Should().Be(2);
            results.Should().ContainSingle(o => o.CurrentPrice == 10 && o.CompanyId == 1);
            results.Should().ContainSingle(o => o.CurrentPrice == 40 && o.CompanyId == 2);
        }

        [Fact]
        public void Should_correctly_compute_aggregate_prices()
        {
            SetupTransactions(
                new UserTransaction { CompanyId = 1, Price = 10, Quantity = 3, UserId = userId }, // 30
                new UserTransaction { CompanyId = 1, Price = 5, Quantity = -2, UserId = userId }, // -10
                new UserTransaction { CompanyId = 1, Price = 20, Quantity = 2, UserId = userId }  // 40
            );

            SetupCurrentPrices(
                new Price { CompanyId = 1, ClosePrice = 10, Date = new DateTime(2016, 12, 1) }
            );

            var results = _service.GetOwnedStocks(userId);

            results.Count.Should().Be(1);
            results.Should().ContainSingle(o => o.CompanyId == 1 
                && o.TotalBuyPrice == 60
                && o.CurrentValue == 30
                && o.OwnedStocksCount == 3);

        }

        private void SetupTransactions(params UserTransaction[] transactions)
        {
            _transactionsRepository.Setup(r => r.GetQueryable(null, null, null, null, null))
                .Returns(transactions.AsQueryable());
        }

        private void SetupCurrentPrices(params Price[] prices)
        {
            var companyIds = prices.Select(p => p.CompanyId).Distinct().OrderBy(c => c).ToList();
            _priceService.Setup(p => p.GetCurrentPrices(It.Is<IList<int>>(c => c.OrderBy(cc => cc).SequenceEqual(companyIds))))
                .Returns(prices);
        }
    }
}
