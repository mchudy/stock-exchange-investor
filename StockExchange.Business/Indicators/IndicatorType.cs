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
        
    }
}
