using System.ComponentModel;

namespace StockExchange.Business.Indicators
{
    public enum IndicatorType : byte
    {
        [Description("ATR")]
        Atr = 1,

        [Description("MACD")]
        Macd = 2,

        [Description("OBV")]
        Obv = 3,

        [Description("ROC")]
        Roc = 4,

        [Description("RSI")]
        Rsi = 5
    }
}
