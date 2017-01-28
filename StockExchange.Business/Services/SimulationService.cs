using StockExchange.Business.Exceptions;
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
            if (simulationDto.SelectedCompanyIds == null || !simulationDto.SelectedCompanyIds.Any())
                throw new BusinessException("No companies were specified");

            var signalEvents = await _indicatorsService.GetSignals(simulationDto.StartDate, simulationDto.EndDate,
                simulationDto.SelectedCompanyIds, strategy.Indicators, strategy.IsConjunctiveStrategy, strategy.SignalDaysPeriod ?? 1);

            var allPrices = await _priceService.GetPrices(simulationDto.SelectedCompanyIds);
            foreach (var signalEvent in signalEvents.OrderBy(item => item.Date))
            {
                // Shall we buy and sell companies thesame day?
                if (signalEvent.CompaniesToSell.Count > 0)
                {
                    HandleSellSignals(simulationDto, allPrices, signalEvent, simulationResult);
                }
                if (signalEvent.CompaniesToBuy.Count > 0)
                {
                    HandleBuySignals(simulationDto, allPrices, signalEvent, simulationResult);
                }
            }
            var keepStrategyProfit = BuyAndKeepStrategyProfit(simulationDto, simulationResult, allPrices);
            simulationResult.SimulationTotalValue = simulationResult.CurrentCompanyQuantity.Sum(x =>
            {
                var companyPricesDto = allPrices.FirstOrDefault(item => item.Company.Id == x.Key);
                var lastDayPrice = companyPricesDto?.Prices.Where(item => item.Date <= simulationDto.EndDate)
                    .OrderByDescending(item => item.Date)
                    .FirstOrDefault();
                if (lastDayPrice != null)
                    return x.Value * lastDayPrice.ClosePrice;
                return 0;
            }) + simulationDto.Budget;
            simulationResult.PercentageProfit = Math.Round((double)((simulationResult.SimulationTotalValue - simulationResult.StartBudget) / simulationResult.StartBudget) * 100, 2);
            CalculateMinimalAndMaximalSimulationValue(simulationDto.StartDate, simulationDto.EndDate, allPrices, simulationDto.SelectedCompanyIds, simulationResult);
            CalculateAverageGainAndLossOnTransaction(simulationResult, simulationDto.SelectedCompanyIds);
            simulationResult.KeepStrategyProfit = keepStrategyProfit;
            return simulationResult;
        }

        private static decimal BuyAndKeepStrategyProfit(SimulationDto simulationDto, SimulationResultDto simulationResult,
            IList<CompanyPricesDto> allPrices)
        {
            var budget = simulationResult.StartBudget;
            var budgetPerCompany = budget / allPrices.Count;
            foreach (var companyPricesDto in allPrices)
            {
                var startDayPrice =
                    companyPricesDto.Prices.Where(item => item.Date >= simulationDto.StartDate)
                        .OrderBy(item => item.Date)
                        .FirstOrDefault();
                var endDatePrice =
                    companyPricesDto.Prices.Where(item => item.Date <= simulationDto.EndDate)
                        .OrderByDescending(item => item.Date)
                        .FirstOrDefault();
                if (startDayPrice == null || endDatePrice == null) continue;
                var quantity = (int)Math.Floor(budgetPerCompany / startDayPrice.ClosePrice);
                budget += quantity * (endDatePrice.ClosePrice - startDayPrice.ClosePrice);
            }
            var keepStrategyProfit = budget - simulationResult.StartBudget;
            return keepStrategyProfit;
        }

        private static void HandleBuySignals(SimulationDto simulationDto, IList<CompanyPricesDto> allPrices, SignalEvent signalEvent,
            SimulationResultDto simulationResult)
        {
            var prices = ConvertPrices(allPrices, signalEvent.CompaniesToBuy, signalEvent.Date)
                .OrderByDescending(item => item.Value);
            foreach (var price in prices)
            {
                var value = simulationDto.Budget;
                if (simulationDto.HasMaximalTransactionLimit)
                    value = Math.Min(value, simulationDto.MaximalBudgetPerTransaction);
                if (simulationDto.HasMinimalTransactionLimit)
                {
                    if(value < simulationDto.MinimalBudgetPerTransaction)
                        continue; // not enough money
                    value = Math.Max(value, simulationDto.MinimalBudgetPerTransaction);
                }
                if (value < price.Value) continue;

                int quantity = (int)Math.Floor(value / price.Value);
                var transaction = new SimulationTransactionDto
                {
                    Date = signalEvent.Date,
                    CompanyId = price.Key,
                    Price = price.Value,
                    Action = SignalAction.Buy,
                    Quantity = quantity,
                    BudgetAfter =
                        simulationDto.Budget - quantity * price.Value
                };
                simulationResult.TransactionsLog.Add(transaction);
                if (simulationResult.CurrentCompanyQuantity.ContainsKey(price.Key))
                    simulationResult.CurrentCompanyQuantity[price.Key] += quantity;
                else
                    simulationResult.CurrentCompanyQuantity.Add(price.Key, quantity);
                simulationDto.Budget = transaction.BudgetAfter;
            }
        }

        private static void HandleSellSignals(SimulationDto simulationDto, IList<CompanyPricesDto> allPrices, SignalEvent signalEvent,
            SimulationResultDto simulationResult)
        {
            var prices = ConvertPrices(allPrices, signalEvent.CompaniesToSell, signalEvent.Date);
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
                simulationDto.Budget = transaction.BudgetAfter;
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
            resultDto.MaximalSimulationValue = new ExtremeSimulationValue(startDate, budget, budget);
            resultDto.MinimalSimulationValue = new ExtremeSimulationValue(startDate, budget, budget);
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

        // Delete me, please
        private static Dictionary<int, decimal> ConvertPrices(IEnumerable<CompanyPricesDto> allPrices, ICollection<int> companyIds, DateTime date)
        {
            return allPrices.Where(p => companyIds.Contains(p.Company.Id) && p.Prices.Any(pr => pr.Date == date))
                // ReSharper disable once PossibleNullReferenceException
                .ToDictionary(p => p.Company.Id, p => p.Prices.FirstOrDefault(pr => pr.Date == date).ClosePrice);
        }

        private static void CalculateAverageGainAndLossOnTransaction(SimulationResultDto resultDto, IList<int> companyIds)
        {
            var transactionDiffs = companyIds.ToDictionary(companyId => companyId, companyId => 0m);
            var gain = 0m;
            var loss = 0m;
            int successes = 0;
            int losses = 0;
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
                    if (diff > 0m)
                    {
                        gain += diff;
                        successes++;
                    }
                    else
                    {
                        loss += diff;
                        losses++;
                    }
                }
            }
            if (successes + losses != 0)
            {
                stats.SuccessTransactionPercentage = Math.Round(100*((double) successes)/(successes + losses), 2);
                stats.FailedTransactionPercentage = Math.Round(100*((double) losses)/(successes + losses), 2);
            }
            else
            {
                stats.SuccessTransactionPercentage = 0;
                stats.FailedTransactionPercentage = 0;
            }
            stats.AverageGainOnTransaction = new AverageTransactionResult(gain, successes);
            stats.AverageLossOnTransaction = new AverageTransactionResult(loss, losses);
            resultDto.TransactionStatistics = stats;
        }
    }
}
