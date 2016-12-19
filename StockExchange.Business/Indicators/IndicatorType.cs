using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Indicators
{
    public enum IndicatorType : byte
    {
        [Display(Name = "ATR (Average True Range)")]
        Atr = 1,

        [Display(Name = "MACD (Moving Average Convergence Divergence)")]
        Macd = 2,

        [Display(Name = "OBV (On-balance Volume)")]
        Obv = 3,

        [Display(Name = "ROC (Rate of Change)")]
        Roc = 4,

        [Display(Name = "RSI (Relative Strength Index)")]
        Rsi = 5,

        [Display(Name = "PP (Pivot Point)")]
        PivotPoint = 6,

        [Display(Name = "SMA (Simple Moving Average)")]
        Sma = 7,

        [Display(Name = "EMA (Expotential Moving Average)")]
        Ema = 8,

        [Display(Name = "VROC (Volume Rate of Change)")]
        Vroc = 9,

    }
}
