using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// A type of the technical indicator
    /// </summary>
    public enum IndicatorType
    {
        /// <summary>
        /// ATR technical indicator
        /// </summary>
        [Display(Name = "ATR (Average True Range)")]
        Atr = 1,

        /// <summary>
        /// MACD technical indicator
        /// </summary>
        [Display(Name = "MACD (Moving Average Convergence Divergence)")]
        Macd = 2,

        /// <summary>
        /// OBV technical indicator
        /// </summary>
        [StrategyIgnoreIndicator]
        [Display(Name = "OBV (On-balance Volume)")]
        Obv = 3,

        /// <summary>
        /// ROC technical indicator
        /// </summary>
        [Display(Name = "ROC (Rate of Change)")]
        Roc = 4,

        /// <summary>
        /// RSI technical indicator
        /// </summary>
        [Display(Name = "RSI (Relative Strength Index)")]
        Rsi = 5,

        /// <summary>
        /// Pivot Point technical indicator
        /// </summary>
        [Display(Name = "PP (Pivot Point)")]
        PivotPoint = 6,

        /// <summary>
        /// SMA technical indicator
        /// </summary>
        [Display(Name = "SMA (Simple Moving Average)")]
        Sma = 7,

        /// <summary>
        /// EMA technical indicator
        /// </summary>
        [Display(Name = "EMA (Expotential Moving Average)")]
        Ema = 8,

        /// <summary>
        /// VROC technical indicator
        /// </summary>
        [StrategyIgnoreIndicator]
        [Display(Name = "VROC (Volume Rate of Change)")]
        Vroc = 9,

        /// <summary>
        /// VHF technical indicator
        /// </summary>
        [StrategyIgnoreIndicator]
        [Display(Name = "VHF (Vertical Horizontal Filter)")]
        Vhf = 10,

        /// <summary>
        /// VPT technical indicator
        /// </summary>
        [StrategyIgnoreIndicator]
        [Display(Name = "VPT (Volume Price Trend)")]
        Vpt = 11,

        /// <summary>
        /// ADX technical indicator
        /// </summary>
        [Display(Name = "ADX (Average Directional Movement Index)")]
        Adx = 12
    }
}
