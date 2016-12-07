using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators
{
    public class RocIndicator : IIndicator
    {
        public const int DefaultRocTerm = 12;

        public int Term { get; set; } = DefaultRocTerm;

        public IndicatorType Type => IndicatorType.Roc;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>();
            for (var i = Term; i < prices.Count; i++)
            {
                var value = (prices[i].ClosePrice - prices[i - Term].ClosePrice) / prices[i - Term].ClosePrice * 100;
                values.Add(new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = value
                });
            }
            return values;
        }
    }
}