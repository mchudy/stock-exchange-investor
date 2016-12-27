using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    public class PpIndicator : IIndicator
    {
        public IndicatorType Type => IndicatorType.PivotPoint;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            return prices.Select(price => new IndicatorValue
            {
                Date = price.Date, Value = (price.HighPrice + price.LowPrice + price.ClosePrice)/3
            }).ToList();
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            List<Signal> signals = new List<Signal>();
            return signals;
        }
    }
}
