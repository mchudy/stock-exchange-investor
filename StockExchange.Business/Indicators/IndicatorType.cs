using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators
{
    public enum IndicatorType : byte
    {
        Atr = 1,
        Macd = 2,
        Obv = 3,
        Roc = 4,
        Rsi = 5
    }
}
