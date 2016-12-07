using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators
{
    public class ObvIndicator : IIndicator
    {
        public IndicatorType Type => IndicatorType.Obv;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>
            {
                new IndicatorValue { Date = prices[0].Date, Value = prices[0].Volume }
            };
            var lastValue = prices[0].Volume;
            for (var i = 1; i < prices.Count; i++)
            {
                var value = lastValue;
                var todayClosePrice = prices[i].ClosePrice;
                var previousClosePrice = prices[i - 1].ClosePrice;
                if (todayClosePrice > previousClosePrice)
                {
                    value += prices[i].Volume;
                }
                else if (todayClosePrice < previousClosePrice)
                {
                    value -= prices[i].Volume;
                }
                lastValue = value;
                values.Add(new IndicatorValue { Date = prices[i].Date, Value = value });
            }
            return values;
        }
    }
}
