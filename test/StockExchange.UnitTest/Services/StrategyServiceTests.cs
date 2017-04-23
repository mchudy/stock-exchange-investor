using FluentAssertions;
using Moq;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Strategy;
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
    public class StrategyServiceTests
    {
        private readonly IStrategyService _service;
        private readonly Mock<IStrategiesRepository> _strategyRepository = new Mock<IStrategiesRepository>();
        private readonly Mock<IIndicatorsService> _indicatorsService = new Mock<IIndicatorsService>();

        private readonly IList<InvestmentStrategy> _strategies = new List<InvestmentStrategy>
        {
            new InvestmentStrategy
            {
                Id = 1,
                UserId = 1,
                Name = "Strategy1"
            }
        };

        public StrategyServiceTests()
        {
            _service = new StrategyService(_strategyRepository.Object, _indicatorsService.Object);
            _strategyRepository.Setup(s => s.GetQueryable())
                .Returns(_strategies.GetTestAsyncQueryable());
        }

        [Fact]
        public void When_user_already_has_strategy_with_given_name_should_throw()
        {
            var newStrategy = new StrategyDto
            {
                Name = "Strategy1",
                UserId = 1
            };
            _strategyRepository.Setup(s => s.StrategyExists(1, "Strategy1")).Returns(System.Threading.Tasks.Task.FromResult(true));

            Func<System.Threading.Tasks.Task> act = async () => await _service.CreateStrategy(newStrategy);
            act.ShouldThrow<BusinessException>();
        }
    }
}
