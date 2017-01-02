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
            var simulationResult = new SimulationResultDto
            {
                TransactionsLog = new List<SimulationTransactionDto>(),
                CurrentCompanyQuantity = new Dictionary<int, int>(),
                StartBudget = simulationDto.Budget,
            };
            var strategy = _strategyService.GetUserStrategy(simulationDto.UserId, simulationDto.SelectedStrategyId);
            if (simulationDto.SelectedCompanyIds == null)
                simulationDto.SelectedCompanyIds = _companyService.GetAllCompanies().Select(item => item.Id).ToList();

            var signalEvents = _indicatorsService.GetSignals(simulationDto.StartDate, simulationDto.EndDate,
                simulationDto.SelectedCompanyIds, strategy.Indicators);

            var allPrices = _priceService.GetPricesForCompanies(simulationDto.SelectedCompanyIds);
            foreach (var signalEvent in signalEvents.OrderBy(item => item.Date))
            {
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
                if (signalEvent.CompaniesToBuy.Count > 0)
                {
                    var prices = ConvertPrices(allPrices, signalEvent.CompaniesToBuy, signalEvent.Date)
                        .OrderByDescending(item => item.Value); //think over that sort
                    foreach (var price in prices)
                    {
                        if (simulationDto.Budget <= price.Value) continue;
                        simulationResult.TransactionsLog.Add(new SimulationTransactionDto
                        {
                            Date = signalEvent.Date,
                            CompanyId = price.Key,
                            Price = price.Value,
                            Action = SignalAction.Buy,
                            Quantity = (int)Math.Floor(simulationDto.Budget / price.Value), //add company stocks budget limit
                            BudgetAfter =
                                simulationDto.Budget - (int)Math.Floor(simulationDto.Budget / price.Value) * price.Value
                        });
                        if (simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key))
                            simulationResult.CurrentCompanyQuantity[price.Key] +=
                                (int)Math.Floor(simulationDto.Budget / price.Value);
                        else
                            simulationResult.CurrentCompanyQuantity.Add(price.Key,
                                (int)Math.Floor(simulationDto.Budget / price.Value));
                        simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                    }
                }
            }
            var currentPrices = _priceService.GetCurrentPrices(simulationResult.CurrentCompanyQuantity.Keys.ToList()).ToDictionary(x => x.CompanyId);
            simulationResult.SimulationTotalValue = simulationResult.CurrentCompanyQuantity.Sum(x => x.Value * currentPrices[x.Key].ClosePrice) + simulationDto.Budget;
            simulationResult.PercentageProfit = Math.Round((double)((simulationResult.SimulationTotalValue - simulationResult.StartBudget) / simulationResult.StartBudget) * 100, 2);
            CalculateMaximalGainAndLossOnTransaction(simulationResult);
            return simulationResult;
        }

        private void CalculateMaximalGainAndLossOnTransaction(SimulationResultDto dto)
        {
            decimal lastSellValue = dto.StartBudget;
            decimal mindiff = 0m;
            decimal maxdiff = 0m;
            foreach (var transaction in dto.TransactionsLog)
            {
                if (transaction.Action == SignalAction.Sell)
                {
                    var sellValue = transaction.BudgetAfter;
                    var diff = (sellValue - lastSellValue)/lastSellValue;
                    if (diff > maxdiff)
                    {
                        dto.MaximalGainOnTransaction = new ExtremeTransactionResult(transaction.Date, lastSellValue, sellValue);
                        maxdiff = diff;
                    }
                    if (diff < mindiff)
                    {
                        dto.MaximalLossOnTransaction = new ExtremeTransactionResult(transaction.Date, lastSellValue, sellValue);
                        mindiff = diff;
                    }
                    lastSellValue = sellValue;
                }
            }
        }

        private Dictionary<int, decimal> ConvertPrices(IList<CompanyPricesDto> allPrices, IList<int> companyIds, DateTime date)
        {
            return allPrices.Where(p => companyIds.Contains(p.Company.Id) && p.Prices.Any(pr => pr.Date == date))
                // ReSharper disable once PossibleNullReferenceException
                .ToDictionary(p => p.Company.Id, p => p.Prices.FirstOrDefault(pr => pr.Date == date).ClosePrice);
        }
    }
}
