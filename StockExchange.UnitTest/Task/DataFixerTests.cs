using FluentAssertions;
using Moq;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StockExchange.UnitTest.Task
{
    public class DataFixerTests
    {
        private readonly IDataFixer _dataFixer;
        private readonly Mock<IRepository<Price>> _priceRepository = new Mock<IRepository<Price>>();
        private readonly List<Price> _prices = new List<Price>();

        public DataFixerTests()
        {
            _dataFixer = new DataFixer(_priceRepository.Object);
            _priceRepository.Setup(r => r.GetQueryable()).Returns(_prices.AsQueryable());
        }

        [Fact]
        public void Should_not_touch_correct_records()
        {
            var _testPrices = new List<Price>
            {
                new Price {CompanyId = 1, HighPrice = 10, LowPrice = 2, ClosePrice = 5, OpenPrice = 7},
                new Price {CompanyId = 1, HighPrice = 10, LowPrice = 10, ClosePrice = 5, OpenPrice = 7},
                new Price {CompanyId = 1, HighPrice = 10, LowPrice = 2, ClosePrice = 5, OpenPrice = 10},
                new Price {CompanyId = 1, HighPrice = 10, LowPrice = 10, ClosePrice = 10, OpenPrice = 10}
            };
            _prices.AddRange(_testPrices);

            _dataFixer.FixData();

            _prices.Should().BeEquivalentTo(_testPrices);
            VerifyNoEntriesRemoved();
        }

        [Fact]
        public void Should_delete_records_where_all_prices_except_close_price_are_zero_()
        {
            var incorrectPrice = new Price {CompanyId = 1, HighPrice = 0, LowPrice = 0, ClosePrice = 10, OpenPrice = 0};
            _prices.Add(incorrectPrice);

            _dataFixer.FixData();

            _priceRepository.Verify(r => r.RemoveRange(It.Is((IEnumerable<Price> p) => p.Contains(incorrectPrice))), Times.Once);
        }

        [Fact]
        public void Should_delete_records_where_open_price_is_zero_and_rest_is_positive()
        {
            var incorrectPrice = new Price {CompanyId = 1, HighPrice = 10, LowPrice = 5, ClosePrice = 8, OpenPrice = 0};
            _prices.Add(incorrectPrice);

            _dataFixer.FixData();

            _priceRepository.Verify(r => r.RemoveRange(It.Is((IEnumerable<Price> p) => p.Contains(incorrectPrice))), Times.Once);
        }

        [Fact]
        public void Should_not_delete_where_open_price_is_not_zero()
        {
            var _testPrices = new List<Price>
            {
                new Price {CompanyId = 1, HighPrice = 0, LowPrice = 0, ClosePrice = 10, OpenPrice = 3},
            };
            _prices.AddRange(_testPrices);

            _dataFixer.FixData();

            _prices.Should().BeEquivalentTo(_testPrices);
            VerifyNoEntriesRemoved();
        }

        private void VerifyNoEntriesRemoved()
        {
            _priceRepository.Verify(r => r.RemoveRange(It.Is((IEnumerable<Price> p) => !p.Any())));
            _priceRepository.Verify(r => r.Remove(It.IsAny<Price>()), Times.Never);
        }
    }
}
