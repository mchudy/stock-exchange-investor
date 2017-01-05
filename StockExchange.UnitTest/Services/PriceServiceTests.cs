using FluentAssertions;
using Moq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class PriceServiceTests
    {
        private readonly IPriceService _service;
        private readonly Mock<IRepository<Price>> _priceRepository = new Mock<IRepository<Price>>();

        public PriceServiceTests()
        {
            _service = new PriceService(_priceRepository.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_return_correct_current_prices_for_single_company()
        {
            SetupPrices(
                new Price { CompanyId = 1, ClosePrice = 10, Date = new DateTime(2016, 12, 1) },
                new Price { CompanyId = 1, ClosePrice = 20, Date = new DateTime(2016, 12, 4) }
            );
            var currentPrices = await _service.GetCurrentPrices(new List<int> { 1 });
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
            var currentPrices = await _service.GetCurrentPrices(new List<int> { 1, 2 });
            currentPrices.Count.Should().Be(2);
            currentPrices.Should().ContainSingle(p => p.CompanyId == 1 && p.ClosePrice == 20);
            currentPrices.Should().ContainSingle(p => p.CompanyId == 2 && p.ClosePrice == 40);
        }

        private void SetupPrices(params Price[] prices)
        {
            _priceRepository.Setup(p => p.GetQueryable())
                .Returns(prices.GetTestAsyncQueryable());
        }
    }
}
