using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Indicators.Common.Intersections;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Average Directional Movement Index technical indicator
    /// </summary>
    public class AdxIndicator : IIndicator
    {
        private const decimal AdxStrongTrendValue = 25m;
        
        /// <summary>
        /// Default <see cref="Term"/> value for the ADX indicator
        /// </summary>
        public const int DefaultTerm = 14;

        /// <summary>
        /// The number of prices from previous days to include when computing 
        /// an indicator value
        /// </summary>
        public int Term { get; set; } = DefaultTerm;

        /// <inheritdoc />
        public IndicatorType Type => IndicatorType.Adx;

        /// <inheritdoc />
        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            IList<IndicatorValue> sma, diplus, diminus, diDifferences, diDifferences2;
            CalculateHelper(prices, out sma, out diplus, out diminus, out diDifferences, out diDifferences2);
            return sma;
        }

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            IList<IndicatorValue> sma, diplus, diminus, diDifferences, diDifferences2;
            CalculateHelper(prices, out sma, out diplus, out diminus, out diDifferences, out diDifferences2);
            var adxValues = sma.Select((t, i) => new IndicatorValue { Date = t.Date, Value = 100 * t.Value / diDifferences2[i].Value }).ToList();
            var intersections = IntersectionHelper.FindIntersections(diplus, diminus);
            var signals = new List<Signal>();
            foreach (var intersectionInfo in intersections)
            {
                SignalAction action = SignalAction.NoSignal;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (intersectionInfo.IntersectionType)
                {
                    case IntersectionType.FirstAbove:
                        action = SignalAction.Buy;
                        break;
                    case IntersectionType.SecondAbove:
                        action = SignalAction.Sell;
                        break;
                    case IntersectionType.Same:
                        action = SignalAction.NoSignal;
                        break;
                }
                var adx = adxValues.FirstOrDefault(v => v.Date == intersectionInfo.Date);
                if(adx != null && adx.Value > AdxStrongTrendValue)
                    signals.Add(new Signal(action) {Date = adx.Date});
            }
            return signals;
        }

        private void CalculateHelper(IList<Price> prices,
            out IList<IndicatorValue> smaValues,
            out IList<IndicatorValue> diplus,
            out IList<IndicatorValue> diminus,
            out IList<IndicatorValue> differences,
            out IList<IndicatorValue> differences2)
        {
            var plusDms = new List<IndicatorValue>();
            var minusDms = new List<IndicatorValue>();
            for (var i = 1; i < prices.Count; ++i)
            {
                decimal plusDm, minusDm;
                var upMove = prices[i].HighPrice - prices[i - 1].HighPrice;
                var downMove = prices[i - 1].LowPrice - prices[i].LowPrice;
                if (upMove > downMove && upMove > 0)
                    plusDm = upMove;
                else
                    plusDm = 0;
                if (downMove > upMove && downMove > 0)
                    minusDm = downMove;
                else
                    minusDm = 0;
                plusDms.Add(new IndicatorValue { Date = prices[i].Date, Value = plusDm });
                minusDms.Add(new IndicatorValue { Date = prices[i].Date, Value = minusDm });
            }
            var plusDisMovingAverage = MovingAverageHelper.SmoothedSum(plusDms, Term);
            var minusDisMovingAverage = MovingAverageHelper.SmoothedSum(minusDms, Term);
            var atr = MovingAverageHelper.SmoothedSum(GetRsValues(prices.Skip(1).ToList()), Term);
            var plusDis = new List<IndicatorValue>();
            var minusDis = new List<IndicatorValue>();
            var diDifferences = new List<IndicatorValue>();
            var diDifferences2 = new List<IndicatorValue>();
            var dxs = new List<IndicatorValue>();
            for (var i = 0; i < atr.Count; ++i)
            {
                plusDis.Add(new IndicatorValue { Date = atr[i].Date, Value = 100 * plusDisMovingAverage[i].Value / atr[i].Value });
                minusDis.Add(new IndicatorValue { Date = atr[i].Date, Value = 100 * minusDisMovingAverage[i].Value / atr[i].Value });
                diDifferences.Add(new IndicatorValue { Date = plusDis[i].Date, Value = Math.Abs(plusDis[i].Value - minusDis[i].Value) });
                diDifferences2.Add(new IndicatorValue { Date = plusDis[i].Date, Value = plusDis[i].Value + minusDis[i].Value });
                dxs.Add(new IndicatorValue { Date = atr[i].Date, Value = 100 * diDifferences[i].Value / diDifferences2[i].Value });
            }
            var sma = MovingAverageHelper.SmoothedMovingAverage2(dxs, Term);
            diDifferences2 = diDifferences2.Skip(Term - 1).ToList();
            smaValues = sma;
            diplus = plusDis;
            diminus = minusDis;
            differences = diDifferences;
            differences2 = diDifferences2;
        }

        private static List<IndicatorValue> GetRsValues(IList<Price> prices)
        {
            var rsValues = new List<IndicatorValue>
            {
                new IndicatorValue
                {
                    Date = prices[0].Date,
                    Value = prices[0].HighPrice - prices[0].LowPrice
                }
            };
            for (var i = 1; i < prices.Count; i++)
            {
                var rs = Math.Max(
                    prices[i].HighPrice - prices[i].LowPrice, Math.Max(
                        Math.Abs(prices[i].HighPrice - prices[i - 1].ClosePrice),
                        Math.Abs(prices[i].LowPrice - prices[i - 1].ClosePrice)));
                rsValues.Add(new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = rs
                });
            }
            return rsValues;
        }
    }
}