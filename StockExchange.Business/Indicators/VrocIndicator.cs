using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    public class VrocIndicator : IIndicator
    {
        public const int DefaultVrocTerm = 14;

        public int Term { get; set; } = DefaultVrocTerm;

        public IndicatorType Type => IndicatorType.Vroc;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>();
            for (var i = Term; i < prices.Count; i++)
            {
                var value = (prices[i].Volume - prices[i - Term].Volume) / prices[i - Term].Volume * 100;
                values.Add(new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = value
                });
            }
            return values;
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            return signals;
        }
    }
}