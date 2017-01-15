using FluentAssertions;
using Moq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.Cache;
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
        private readonly Mock<ITransactionsService> _transactionsService = new Mock<ITransactionsService>();
        private readonly Mock<IPriceService> _priceService = new Mock<IPriceService>();
        private readonly Mock<ICache> _cache = new Mock<ICache>();

        public WalletServiceTests()
        {
            _service = new WalletService(_transactionsService.Object, _priceService.Object, _cache.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_return_only_companies_which_stocks_user_currently_owns()
        {
            var d = new Dictionary<int, List<UserTransaction>>
            {
                //{
                //    1, new List<UserTransaction>
                //    {
                //        new UserTransaction {CompanyId = 1, Price = 10, Quantity = 1, UserId = userId},
                //        new UserTransaction {CompanyId = 1, Price = 20, Quantity = -1, UserId = userId}
                //    }
                //},
                {
                    2, new List<UserTransaction>
                    {
                        new UserTransaction {CompanyId = 2, Price = 5, Quantity = 2, UserId = userId}
                    }
                }
            };
            SetupTransactions(d);
            SetupCurrentPrices(new Price { CompanyId = 2, ClosePrice = 10 });
            var results = await _service.GetOwnedStocks(userId);
            results.Count.Should().Be(1);
            results[0].CompanyId.Should().Be(2);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_set_current_prices_to_correct_values()
        {
            var d = new Dictionary<int, List<UserTransaction>>
            {
                {
                    1, new List<UserTransaction>
                    {
                        new UserTransaction {CompanyId = 1, Price = 10, Quantity = 1, UserId = userId},
                    }
                },
                {
                    2, new List<UserTransaction>
                    {
                        new UserTransaction {CompanyId = 2, Price = 10, Quantity = 2, UserId = userId}
                    }
                }
            };
            SetupTransactions(d);
            SetupCurrentPrices(
                new Price { CompanyId = 1, ClosePrice = 10 },
                new Price { CompanyId = 2, ClosePrice = 40 }
            );
            var results = await _service.GetOwnedStocks(userId);
            results.Count.Should().Be(2);
            results.Should().ContainSingle(o => o.CurrentPrice == 10 && o.CompanyId == 1);
            results.Should().ContainSingle(o => o.CurrentPrice == 40 && o.CompanyId == 2);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_correctly_compute_aggregate_prices()
        {
            var d = new Dictionary<int, List<UserTransaction>>
            {
                {
                    1, new List<UserTransaction>
                    {
                        new UserTransaction { CompanyId = 1, Price = 10, Quantity = 3, UserId = userId }, // 30
                        new UserTransaction { CompanyId = 1, Price = 5, Quantity = -2, UserId = userId }, // -10
                        new UserTransaction { CompanyId = 1, Price = 20, Quantity = 2, UserId = userId }  // 40
                     
                    }
                }
            };
            SetupTransactions(d);
            SetupCurrentPrices(
                new Price { CompanyId = 1, ClosePrice = 10, Date = new DateTime(2016, 12, 1) }
            );
            var results = await _service.GetOwnedStocks(userId);
            results.Count.Should().Be(1);
            results.Should().ContainSingle(o => o.CompanyId == 1
                && o.TotalBuyPrice == 60
                && o.CurrentValue == 30
                && o.OwnedStocksCount == 3);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_include_transactions_only_for_the_given_user()
        {
            var d = new Dictionary<int, List<UserTransaction>>
            {
                {
                    1, new List<UserTransaction>
                    {
                        new UserTransaction {CompanyId = 1, Price = 10, Quantity = 3, UserId = userId},
                        //new UserTransaction {CompanyId = 1, Price = 10, Quantity = 3, UserId = userId + 1}
                    }
                }
                //,
                //{
                //    2, new List<UserTransaction>
                //    {
                //        new UserTransaction {CompanyId = 2, Price = 5, Quantity = -2, UserId = userId + 1}
                //    }
                //}
            };
            SetupTransactions(d);
            SetupCurrentPrices(
                new Price { CompanyId = 1, ClosePrice = 10 },
                new Price { CompanyId = 2, ClosePrice = 40 }
            );
            var results = await _service.GetOwnedStocks(userId);
            results.Count.Should().Be(1);
            results.Should().ContainSingle(r => r.CompanyId == 1 && r.OwnedStocksCount == 3);
        }

        private void SetupTransactions(Dictionary<int, List<UserTransaction>> transactions)
        {
            _transactionsService.Setup(r => r.GetTransactionsByCompany(userId))
                .Returns(System.Threading.Tasks.Task.FromResult(transactions));
        }

        private void SetupCurrentPrices(params Price[] prices)
        {
            _priceService.Setup(p => p.GetCurrentPrices(It.IsAny<IList<int>>()))
                .Returns(System.Threading.Tasks.Task.FromResult((IList<Price>)prices.ToList()));
        }
    }
}
