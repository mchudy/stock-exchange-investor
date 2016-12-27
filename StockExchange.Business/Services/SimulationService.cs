using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Models.Simulations;

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
            foreach (var signalEvent in signalEvents.OrderBy(item => item.Date))
            {
                var flag = false;
                if (signalEvent.CompaniesToBuy.Count > 0)
                {
                    var prices =
                        _priceService.GetPrices(signalEvent.CompaniesToBuy, signalEvent.Date)
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
                            Quantity = (int) Math.Floor(simulationDto.Budget/price.Value),
                            BudgetAfter =
                                simulationDto.Budget - (int) Math.Floor(simulationDto.Budget/price.Value)*price.Value
                        });
                        simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                        flag = true;
                        if (simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key))
                            simulationResult.CurrentCompanyQuantity[price.Key] +=
                                (int) Math.Floor(simulationDto.Budget/price.Value);
                        else
                            simulationResult.CurrentCompanyQuantity.Add(price.Key,
                                (int) Math.Floor(simulationDto.Budget/price.Value));
                    }
                }
                if (flag) continue;
                // ReSharper disable once InvertIf
                if (signalEvent.CompaniesToSell.Count == 0)
                {
                    var prices =
                        _priceService.GetPrices(signalEvent.CompaniesToSell, signalEvent.Date)
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
                                simulationDto.Budget + simulationResult.CurrentCompanyQuantity[price.Key]*price.Value
                        });
                        simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                        simulationResult.CurrentCompanyQuantity.Remove(price.Key);
                    }
                }
            }
            return simulationResult;
        }
    }
}
