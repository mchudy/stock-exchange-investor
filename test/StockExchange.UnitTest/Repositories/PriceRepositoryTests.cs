using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using Moq;
using StockExchange.DataAccess;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Repositories
{
    public class PriceRepositoryTests
    {
        private IPriceRepository _priceRepository;
        private readonly Mock<StockExchangeModel> _context = new Mock<StockExchangeModel>();

        [Fact]
        public async System.Threading.Tasks.Task Should_return_correct_current_prices_for_single_company()
        {
            SetupPrices(
                new Price { CompanyId = 1, ClosePrice = 10, Date = new DateTime(2016, 12, 1) },
                new Price { CompanyId = 1, ClosePrice = 20, Date = new DateTime(2016, 12, 4) }
            );
            var currentPrices = await _priceRepository.GetCurrentPrices(new List<int> { 1 });
            currentPrices.Count.Should().Be(1);
            currentPrices.Should().ContainSingle(p => p.CompanyId == 1 && p.ClosePrice == 20);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_return_correct_current_prices_for_multiple_companies()
        {
            SetupPrices(
                new Price { CompanyId = 1, ClosePrice = 10, Date = new DateTime(2016, 12, 1) },
                new Price { CompanyId = 1, ClosePrice = 20, Date = new DateTime(2016, 12, 4) },
                new Price { CompanyId = 2, ClosePrice = 40, Date = new DateTime(2016, 12, 8) },
                new Price { CompanyId = 2, ClosePrice = 30, Date = new DateTime(2016, 12, 5) }
            );
            var currentPrices = await _priceRepository.GetCurrentPrices(new List<int> { 1, 2 });
            currentPrices.Count.Should().Be(2);
            currentPrices.Should().ContainSingle(p => p.CompanyId == 1 && p.ClosePrice == 20);
            currentPrices.Should().ContainSingle(p => p.CompanyId == 2 && p.ClosePrice == 40);
        }

        private void SetupPrices(params Price[] prices)
        {
            var pricesMock = new Mock<DbSet<Price>>();
            var asyncPrices = prices.GetTestAsyncQueryable().AsQueryable();
            pricesMock.As<IQueryable<Price>>().Setup(m => m.Provider).Returns(asyncPrices.Provider);
            pricesMock.As<IQueryable<Price>>().Setup(m => m.Expression).Returns(asyncPrices.Expression);
            pricesMock.As<IQueryable<Price>>().Setup(m => m.ElementType).Returns(asyncPrices.ElementType);
            pricesMock.As<IQueryable<Price>>().Setup(m => m.GetEnumerator()).Returns(asyncPrices.GetEnumerator());
            _context.Setup(p => p.Prices).Returns(pricesMock.Object);
            _context.Setup(c => c.Set<Price>()).Returns(pricesMock.Object);
            _priceRepository = new PriceRepository(_context.Object);
        }
    }
}
