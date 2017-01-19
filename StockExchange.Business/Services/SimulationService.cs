using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
using StockExchange.Business.Models.Simulations;
using StockExchange.Business.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for running trading game simulations
    /// </summary>
    public class SimulationService : ISimulationService
    {
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly ICompanyService _companyService;
        private readonly IPriceService _priceService;

        /// <summary>
        /// Creates a new instance of <see cref="SimulationService"/>
        /// </summary>
        /// <param name="strategyService"></param>
        /// <param name="indicatorsService"></param>
        /// <param name="companyService"></param>
        /// <param name="priceService"></param>
        public SimulationService(IStrategyService strategyService, IIndicatorsService indicatorsService, ICompanyService companyService, IPriceService priceService)
        {
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
            _companyService = companyService;
            _priceService = priceService;
        }

        /// <inheritdoc />
        public async Task<SimulationResultDto> RunSimulation(SimulationDto simulationDto)
        {
            var simulationResult = new SimulationResultDto
            {
                TransactionsLog = new List<SimulationTransactionDto>(),
                CurrentCompanyQuantity = new Dictionary<int, int>(),
                StartBudget = simulationDto.Budget,
            };
            var strategy = await _strategyService.GetStrategy(simulationDto.UserId, simulationDto.SelectedStrategyId);
            if (simulationDto.SelectedCompanyIds == null)
                simulationDto.SelectedCompanyIds = (await _companyService.GetCompanies()).Select(item => item.Id).ToList();

            var signalEvents = await _indicatorsService.GetSignals(simulationDto.StartDate, simulationDto.EndDate,
                simulationDto.SelectedCompanyIds, strategy.Indicators);

            var allPrices = await _priceService.GetPrices(simulationDto.SelectedCompanyIds);
            foreach (var signalEvent in signalEvents.OrderBy(item => item.Date))
            {
                if (signalEvent.CompaniesToSell.Count > 0)
                {
                    HandleSellSignals(simulationDto, allPrices, signalEvent, simulationResult);
                }
                if (signalEvent.CompaniesToBuy.Count > 0)
                {
                    HandleBuySignals(simulationDto, allPrices, signalEvent, simulationResult);
                }
            }
            var currentPrices = (await _priceService.GetCurrentPrices(simulationResult.CurrentCompanyQuantity.Keys.ToList())).ToDictionary(x => x.CompanyId);
            simulationResult.SimulationTotalValue = simulationResult.CurrentCompanyQuantity.Sum(x => x.Value * currentPrices[x.Key].ClosePrice) + simulationDto.Budget;
            simulationResult.PercentageProfit = Math.Round((double)((simulationResult.SimulationTotalValue - simulationResult.StartBudget) / simulationResult.StartBudget) * 100, 2);
            CalculateMinimalAndMaximalSimulationValue(simulationDto.StartDate, simulationDto.EndDate, allPrices, simulationDto.SelectedCompanyIds, simulationResult);
            CalculateMaximalGainAndLossOnTransaction(simulationResult, simulationDto.SelectedCompanyIds);
            return simulationResult;
        }

        private static void HandleBuySignals(SimulationDto simulationDto, IList<CompanyPricesDto> allPrices, SignalEvent signalEvent,
            SimulationResultDto simulationResult)
        {
            var prices = ConvertPrices(allPrices, signalEvent.CompaniesToBuy, signalEvent.Date)
                .OrderByDescending(item => item.Value);
            foreach (var price in prices)
            {
                var value = simulationDto.Budget;
                if (simulationDto.HasTransactionLimit)
                    value = Math.Min(value, simulationDto.MaximalBudgetPerTransaction);
                if (value <= price.Value) continue;
                int quantity = (int)Math.Floor(value / price.Value);
                simulationResult.TransactionsLog.Add(new SimulationTransactionDto
                {
                    Date = signalEvent.Date,
                    CompanyId = price.Key,
                    Price = price.Value,
                    Action = SignalAction.Buy,
                    Quantity = quantity, //add company stocks budget limit
                    BudgetAfter =
                        simulationDto.Budget - quantity * price.Value
                });
                if (simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key))
                    simulationResult.CurrentCompanyQuantity[price.Key] += quantity;
                else
                    simulationResult.CurrentCompanyQuantity.Add(price.Key, quantity);
                simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
            }
        }

        private static void HandleSellSignals(SimulationDto simulationDto, IList<CompanyPricesDto> allPrices, SignalEvent signalEvent,
            SimulationResultDto simulationResult)
        {
            var prices = ConvertPrices(allPrices, signalEvent.CompaniesToSell, signalEvent.Date)
                .OrderByDescending(item => item.Value);
            foreach (var price in prices)
            {
                if (!simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key)) continue;
                var transaction = new SimulationTransactionDto
                {
                    Date = signalEvent.Date,
                    CompanyId = price.Key,
                    Price = price.Value,
                    Action = SignalAction.Sell,
                    Quantity = simulationResult.CurrentCompanyQuantity[price.Key],
                    BudgetAfter =
                        simulationDto.Budget + simulationResult.CurrentCompanyQuantity[price.Key] * price.Value
                };
                simulationResult.TransactionsLog.Add(transaction);
                simulationDto.Budget = simulationResult.TransactionsLog.Last().BudgetAfter;
                simulationResult.CurrentCompanyQuantity.Remove(price.Key);
            }
        }

        private static void CalculateMinimalAndMaximalSimulationValue(DateTime startDate, DateTime endDate,
            IList<CompanyPricesDto> prices, IList<int> companyIds, SimulationResultDto resultDto)
        {
            Dictionary<int, int> quantities = companyIds.ToDictionary(companyId => companyId, companyId => 0);
            decimal budget = resultDto.StartBudget;
            decimal minVal = resultDto.StartBudget;
            decimal maxVal = resultDto.StartBudget;
            for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
            {
                foreach (var trans in resultDto.TransactionsLog.Where(trans => trans.Date == d))
                {
                    UpdateQuantities(quantities, trans);
                    budget = trans.BudgetAfter;
                }
                var dailyPrices = ConvertPrices(prices, companyIds, d);
                if (dailyPrices.Count == 0) continue;
                decimal value = budget + dailyPrices.Sum(dailyPrice => dailyPrice.Value * quantities[dailyPrice.Key]);
                if (value > maxVal)
                {
                    resultDto.MaximalSimulationValue = new ExtremeSimulationValue(d, value, resultDto.StartBudget);
                    maxVal = value;
                }
                if (value < minVal)
                {
                    resultDto.MinimalSimulationValue = new ExtremeSimulationValue(d, value, resultDto.StartBudget);
                    minVal = value;
                }
            }
        }

        private static void UpdateQuantities(Dictionary<int, int> quantities, SimulationTransactionDto transaction)
        {
            if (transaction.Action == SignalAction.Buy)
            {
                quantities[transaction.CompanyId] += transaction.Quantity;
            }
            else if (transaction.Action == SignalAction.Sell)
            {
                quantities[transaction.CompanyId] -= transaction.Quantity;
            }
        }

        private static Dictionary<int, decimal> ConvertPrices(IEnumerable<CompanyPricesDto> allPrices, ICollection<int> companyIds, DateTime date)
        {
            return allPrices.Where(p => companyIds.Contains(p.Company.Id) && p.Prices.Any(pr => pr.Date == date))
                // ReSharper disable once PossibleNullReferenceException
                .ToDictionary(p => p.Company.Id, p => p.Prices.FirstOrDefault(pr => pr.Date == date).ClosePrice);
        }

        private static void CalculateMaximalGainAndLossOnTransaction(SimulationResultDto resultDto, IList<int> companyIds)
        {
            var transactionDiffs = companyIds.ToDictionary<int, int, decimal>(companyId => companyId, companyId => 0);
            var maxGain = 0m;
            var maxLoss = 0m;
            var successes = 0;
            var losses = 0;
            TransactionStatistics stats = new TransactionStatistics();
            foreach (var trans in resultDto.TransactionsLog)
            {
                if (trans.Action == SignalAction.Buy)
                {
                    transactionDiffs[trans.CompanyId] += trans.Quantity * trans.Price;
                }
                else if (trans.Action == SignalAction.Sell)
                {
                    decimal buyValue = transactionDiffs[trans.CompanyId];
                    decimal sellValue = trans.Quantity * trans.Price;
                    transactionDiffs[trans.CompanyId] = 0;
                    var diff = sellValue - buyValue;
                    if (diff > maxGain)
                    {
                        maxGain = diff;
                        stats.MaximalGainOnTransaction = new ExtremeTransactionResult(trans.Date, buyValue, sellValue);
                    }
                    if (diff < maxLoss)
                    {
                        maxLoss = diff;
                        stats.MaximalLossOnTransaction = new ExtremeTransactionResult(trans.Date, buyValue, sellValue);
                    }
                    if (diff > 0m)
                        successes++;
                    else
                        losses++;
                }
            }
            stats.SuccessTransactionPercentage = Math.Round(100*((double) successes)/(successes + losses), 2);
            stats.FailedTransactionPercentage = Math.Round(100*((double) losses)/(successes + losses), 2);
            resultDto.TransactionStatistics = stats;
        }
    }
}
