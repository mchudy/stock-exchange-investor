using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Models;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Services
{
    public class StrategyService : IStrategyService
    {
        private readonly IRepository<InvestmentStrategy> _strategiesRepository;

        public StrategyService(IRepository<InvestmentStrategy> strategiesRepository)
        {
            _strategiesRepository = strategiesRepository;
        }

        public void CreateStrategy(StrategyDto strategy)
        {
            InvestmentStrategy investmentStrategy = new InvestmentStrategy()
            {
                UserId = strategy.UserId,
            };
            //_strategiesRepository.Insert(investmentStrategy);
            //_strategiesRepository.Save();
        }
    }
}
