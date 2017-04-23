using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Provides methods for computing moving averages
    /// </summary>
    public static class MovingAverageHelper
    {
        /// <summary>
        /// Computes a simple moving average
        /// </summary>
        /// <param name="prices">Prices for which the average should be computed</param>
        /// <returns>Value of the simple moving average</returns>
        public static IndicatorValue SimpleMovingAverage(IList<Price> prices)
        {
            if (prices == null || prices.Count == 0 || !CheckDates(prices.Select(pr => pr.Date).ToList()))
                throw new IndicatorArgumentException();

            return new IndicatorValue
            {
                Date = prices.Last().Date,
                Value = prices.Select(x => x.ClosePrice).Average()
            };
        }

        /// <summary>
        /// Computes a simple moving average
        /// </summary>
        /// <param name="values">Indicator values for which the average should be computed</param>
        /// <returns>Value of the simple moving average</returns>
        public static IndicatorValue SimpleMovingAverage(IList<IndicatorValue> values)
        {
            if (values == null || values.Count == 0 || !CheckDates(values.Select(v => v.Date).ToList()))
                throw new IndicatorArgumentException();

            return new IndicatorValue
            {
                Date = values.Last().Date,
                Value = values.Select(x => x.Value).Average()
            };
        }

        /// <summary>
        /// Computes an exponential moving average
        /// </summary>
        /// <param name="prices">Prices for which the average should be computed</param>
        /// <param name="terms">A period in days</param>
        /// <returns>Values of the exponential moving average</returns>
        public static IList<IndicatorValue> ExpotentialMovingAverage(IList<Price> prices, int terms)
        {
            if (prices == null || prices.Count < terms || terms < 1 || !CheckDates(prices.Select(price => price.Date).ToList()))
                throw new IndicatorArgumentException();
            IList<IndicatorValue> averages = new List<IndicatorValue>();
            var ema = SimpleMovingAverage(prices.Take(terms).ToList());
            averages.Add(ema);
            var alpha = 2.0m / (terms + 1);
            var p = 1 - alpha;
            for (var i = terms; i < prices.Count; i++)
            {
                var nextEma = new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = prices[i].ClosePrice * alpha + ema.Value * p
                };
                ema = nextEma;
                averages.Add(ema);
            }
            return averages;
        }

        /// <summary>
        /// Computes an exponential moving average
        /// </summary>
        /// <param name="values">Indicator values for which the average should be computed</param>
        /// <param name="terms">A period in days</param>
        /// <returns>Values of the exponential moving average</returns>
        public static IList<IndicatorValue> ExpotentialMovingAverage(IList<IndicatorValue> values, int terms)
        {
            if (!IsInputValid(values, terms))
                throw new IndicatorArgumentException();

            var alpha = 2.0m / (terms + 1);
            return ExponentialMovingAverageInternal(values, terms, alpha);
        }

        /// <summary>
        /// Computes a smoothed moving average (SSMA)
        /// </summary>
        /// <param name="values">Indicator values for which the average should be computed</param>
        /// <param name="terms">A period in days</param>
        /// <returns>Values of the smoothed moving average</returns>
        public static IList<IndicatorValue> SmoothedMovingAverage(IList<IndicatorValue> values, int terms)
        {
            if (!IsInputValid(values, terms))
                throw new IndicatorArgumentException();

            var alpha = 1m / terms;
            return ExponentialMovingAverageInternal(values, terms, alpha);
        }

        /// <summary>
        /// Computes a smoothed sum
        /// </summary>
        /// <param name="indicatorValues">Indicator values for which the sum should be computed</param>
        /// <param name="terms">A period in days</param>
        /// <returns>Values of the smoothed sum</returns>
        public static IList<IndicatorValue> SmoothedSum(IList<IndicatorValue> indicatorValues, int terms)
        {
            var values = new List<IndicatorValue>();
            var prev = new IndicatorValue()
            {
                Date = indicatorValues[terms - 1].Date,
                Value = indicatorValues.Take(terms).Sum(x => x.Value)
            };
            values.Add(prev);
            for (int i = terms; i < indicatorValues.Count; i++)
            {
                var newVal = new IndicatorValue()
                {
                    Date = indicatorValues[i].Date,
                    Value = prev.Value - (prev.Value/terms) + indicatorValues[i].Value
                };
                values.Add(newVal);
                prev = newVal;
            }
            return values;
        }

        /// <summary>
        /// Computes a smoothed moving average (SSMA)
        /// </summary>
        /// <param name="indicatorValues">Indicator values for which the average should be computed</param>
        /// <param name="terms">A period in days</param>
        /// <returns>Values of the smoothed moving average</returns>
        public static IList<IndicatorValue> SmoothedMovingAverage2(IList<IndicatorValue> indicatorValues, int terms)
        {
            var values = new List<IndicatorValue>();
            var prev = new IndicatorValue()
            {
                Date = indicatorValues[terms - 1].Date,
                Value = indicatorValues.Take(terms).Average(x => x.Value)
            };
            values.Add(prev);
            for (int i = terms; i < indicatorValues.Count; i++)
            {
                var newVal = new IndicatorValue()
                {
                    Date = indicatorValues[i].Date,
                    Value = (prev.Value * (terms-1) + indicatorValues[i].Value)/terms
                };
                values.Add(newVal);
                prev = newVal;
            }
            return values;
        }

        internal static IList<Signal> GenerateSignalsForMovingAverages(IIndicator indicator, int term, IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = indicator.Calculate(prices);
            for (int i = term; i < prices.Count; i++)
            {
                if (values[i - term].Value < values[i - term + 1].Value && prices[i].ClosePrice > values[i - term + 1].Value)
                    signals.Add(new Signal(SignalAction.Buy) { Date = prices[i].Date });
                else if (values[i - term].Value > values[i - term + 1].Value && prices[i].ClosePrice < values[i - term + 1].Value)
                    signals.Add(new Signal(SignalAction.Sell) { Date = prices[i].Date });
            }
            return signals;
        }

        private static IList<IndicatorValue> ExponentialMovingAverageInternal(IList<IndicatorValue> values, int terms, decimal alpha)
        {
            IList<IndicatorValue> averages = new List<IndicatorValue>();
            var ema = SimpleMovingAverage(values.Take(terms).ToList());
            averages.Add(ema);
            for (var i = terms; i < values.Count; i++)
            {
                var nextEma = new IndicatorValue
                {
                    Date = values[i].Date,
                    Value = ema.Value + alpha * (values[i].Value - ema.Value)
                };
                ema = nextEma;
                averages.Add(ema);
            }
            return averages;
        }

        private static bool IsInputValid(ICollection<IndicatorValue> values, int terms)
        {
            return values != null && values.Count >= terms && terms >= 1 &&
                   CheckDates(values.Select(price => price.Date).ToList());
        }

        //Checks whether prices are sorted by date and there are no holes between prices.
        private static bool CheckDates(IList<DateTime> dates)
        {
            for (var i = 1; i < dates.Count; i++)
                if (dates[i].Date <= dates[i - 1].Date)
                    return false;
            return true;
        }
    }
}
