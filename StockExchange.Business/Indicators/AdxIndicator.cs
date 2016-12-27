using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    public class AdxIndicator : IIndicator
    {
        public const int DefaultTerm = 14;

        public IndicatorType Type => IndicatorType.Adx;

        public int Term { get; set; } = DefaultTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
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
            var plusDisMovingAverage = MovingAverageHelper.SmoothedMovingAverage(plusDms, Term);
            var minusDisMovingAverage = MovingAverageHelper.SmoothedMovingAverage(minusDms, Term);
            var atr = new AtrIndicator().Calculate(prices.Skip(1).ToList());
            var plusDis = new List<IndicatorValue>();
            var minusDis = new List<IndicatorValue>();
            var diDifferences = new List<IndicatorValue>();
            var diDifferences2 = new List<IndicatorValue>();
            for (var i = 0; i < atr.Count; ++i)
            {
                plusDis.Add(new IndicatorValue { Date = atr[i].Date, Value = 100 * plusDisMovingAverage[i].Value / atr[i].Value });
                minusDis.Add(new IndicatorValue { Date = atr[i].Date, Value = 100 * minusDisMovingAverage[i].Value / atr[i].Value });
                diDifferences.Add(new IndicatorValue { Date = plusDis[i].Date, Value = Math.Abs(plusDis[i].Value - minusDis[i].Value) });
                diDifferences2.Add(new IndicatorValue { Date = plusDis[i].Date, Value = plusDis[i].Value - minusDis[i].Value });

            }
            var sma = MovingAverageHelper.SmoothedMovingAverage(diDifferences, Term);
            diDifferences2 = diDifferences2.Skip(Term - 1).ToList();
            return sma.Select((t, i) => new IndicatorValue { Date = t.Date, Value = 100 * t.Value / diDifferences2[i].Value }).ToList();
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            return signals;
        }
    }
}
