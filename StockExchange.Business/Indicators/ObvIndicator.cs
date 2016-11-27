using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    public class ObvIndicator : IIndicator
    {
        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>
            {
                new IndicatorValue {Date = prices[0].Date, Value = prices[0].Volume}
            };
            int lastValue = prices[0].Volume;
            for (int i = 1; i < prices.Count; i++)
            {
                int value = lastValue;
                decimal todayClosePrice = prices[i].ClosePrice;
                decimal previousClosePrice = prices[i - 1].ClosePrice;

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
