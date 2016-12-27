using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
using StockExchange.Business.Models.Simulations;
using StockExchange.Business.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly ICompanyService _companyService;
        private readonly IPriceService _priceService;

        public SimulationService(IStrategyService strategyService, IIndicatorsService indicatorsService, ICompanyService companyService, IPriceService priceService)
        {
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
            _companyService = companyService;
            _priceService = priceService;
        }

        public SimulationResultDto RunSimulation(SimulationDto simulationDto)
        {
            var simulationResult = new SimulationResultDto { TransactionsLog = new List<SimulationTransactionDto>(), CurrentCompanyQuantity = new Dictionary<int, int>() };
            var strategy = _strategyService.GetUserStrategy(simulationDto.UserId, simulationDto.SelectedStrategyId);
            if (simulationDto.SelectedCompanyIds == null)
                simulationDto.SelectedCompanyIds = _companyService.GetAllCompanies().Select(item => item.Id).ToList();

            var signalEvents = _indicatorsService.GetSignals(simulationDto.StartDate, simulationDto.EndDate,
                simulationDto.SelectedCompanyIds, strategy.Indicators);

            var allPrices = _priceService.GetPricesForCompanies(simulationDto.SelectedCompanyIds);

            foreach (var signalEvent in signalEvents.OrderBy(item => item.Date))
            {
                var flag = false;
                if (signalEvent.CompaniesToBuy.Count > 0)
                {
                    var prices = ConvertPrices(allPrices, signalEvent.CompaniesToBuy, signalEvent.Date)
                        .OrderByDescending(item => item.Value);
                    foreach (var price in prices)
                    {
                        if (simulationDto.Budget <= price.Value) continue;
                        simulationResult.TransactionsLog.Add(new SimulationTransactionDto
                        {
                            Date = signalEvent.Date,
                            CompanyId = price.Key,
                            Price = price.Value,
                            Action = SignalAction.Buy,
                            Quantity = (int)Math.Floor(simulationDto.Budget / price.Value),
                            BudgetAfter =
                                simulationDto.Budget - (int)Math.Floor(simulationDto.Budget / price.Value) * price.Value
                        });
                        flag = true;
                        if (simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key))
                            simulationResult.CurrentCompanyQuantity[price.Key] +=
                                (int)Math.Floor(simulationDto.Budget / price.Value);
                        else
                            simulationResult.CurrentCompanyQuantity.Add(price.Key,
                                (int)Math.Floor(simulationDto.Budget / price.Value));
                        simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                    }
                }
                if (flag) continue;
                // ReSharper disable once InvertIf
                if (signalEvent.CompaniesToSell.Count > 0)
                {
                    var prices = ConvertPrices(allPrices, signalEvent.CompaniesToSell, signalEvent.Date)
                        .OrderByDescending(item => item.Value);
                    foreach (var price in prices)
                    {
                        if (!simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key)) continue;
                        simulationResult.TransactionsLog.Add(new SimulationTransactionDto
                        {
                            Date = signalEvent.Date,
                            CompanyId = price.Key,
                            Price = price.Value,
                            Action = SignalAction.Sell,
                            Quantity = simulationResult.CurrentCompanyQuantity[price.Key],
                            BudgetAfter =
                                simulationDto.Budget + simulationResult.CurrentCompanyQuantity[price.Key] * price.Value
                        });
                        simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                        simulationResult.CurrentCompanyQuantity.Remove(price.Key);
                    }
                }
            }
            return simulationResult;
        }

        private Dictionary<int, decimal> ConvertPrices(IList<CompanyPricesDto> allPrices, IList<int> companyIds, DateTime date)
        {
            return allPrices.Where(p => companyIds.Contains(p.Company.Id) && p.Prices.Any(pr => pr.Date == date))
                .Select(p => new { Company = p.Company.Id, Price = p.Prices.FirstOrDefault(pr => pr.Date == date)})
                .ToDictionary(p => p.Company, p => (p.Price.ClosePrice + p.Price.HighPrice + p.Price.LowPrice + p.Price.OpenPrice) / 4 );
        }
    }
}
