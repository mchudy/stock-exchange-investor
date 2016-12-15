using StockExchange.Business.Models;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    public class PivotPointIndicator : IIndicator
    {
        public IndicatorType Type => IndicatorType.PivotPoint;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            IList<IndicatorValue> values = new List<IndicatorValue>();
            foreach (var price in prices)
            {
                var val = new IndicatorValue()
                {
                    Date = price.Date,
                    Value = (price.HighPrice + price.LowPrice + price.ClosePrice)/3
                };
                values.Add(val);
            }
            return values;
        }

        public IList<Signal> GenerateSignals(IList<IndicatorValue> values)
        {
            List<Signal> signals = new List<Signal>();
            return signals;
        }
    }
}
