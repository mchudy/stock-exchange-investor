using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    public class VhfIndicator : IIndicator
    {
        public const int DefaultTerm = 14;

        public IndicatorType Type => IndicatorType.Vhf;

        public int Term { get; set; } = DefaultTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var ret = new List<IndicatorValue>();
            for (var i = 0; i < prices.Count - Term; ++i)
            {
                var current = prices.Skip(i).Take(Term).ToList();
                var max = current.Max(item => item.ClosePrice);
                var min = current.Min(item => item.ClosePrice);
                var sum = 0m;
                for (var j = 1; j < current.Count; ++j)
                {
                    sum += Math.Abs(current[j].ClosePrice - current[j - 1].ClosePrice);
                }
                var vhf = sum != 0 ? Math.Abs(max - min) / sum : 0m;
                ret.Add(new IndicatorValue
                {
                    Value = vhf,
                    Date = current.Last().Date
                });
            }
            return ret;
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            return signals;
        }
    }
}
