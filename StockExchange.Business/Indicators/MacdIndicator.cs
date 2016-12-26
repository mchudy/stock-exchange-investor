using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Wskaźnik Macd
    /// </summary>
    public class MacdIndicator : IIndicator
    {
        public const int DefaultLongTerm = 26;
        public const int DefaultShortTerm = 12;
        public const int DefaultSignalTerm = 9;

        public int LongTerm { get; set; } = DefaultLongTerm;

        public int ShortTerm { get; set; } = DefaultShortTerm;

        public int SignalTerm { get; set; } = DefaultSignalTerm;

        public IndicatorType Type => IndicatorType.Macd;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var longEma = MovingAverageHelper.ExpotentialMovingAverage(prices, LongTerm);
            var shortEma = MovingAverageHelper.ExpotentialMovingAverage(prices, ShortTerm);
            var macdLine = SubstractLongEmaFromShortEma(shortEma, longEma);
            var signalLine = MovingAverageHelper.ExpotentialMovingAverage(macdLine, SignalTerm);
            return PrepareResult(macdLine, signalLine);
        }

        private IList<IndicatorValue> SubstractLongEmaFromShortEma(IList<IndicatorValue> shortEma, IList<IndicatorValue> longEma)
        {
            var difference = LongTerm - ShortTerm;
            IList<IndicatorValue> values = new List<IndicatorValue>();
            for (var i = difference; i < shortEma.Count; i++)
            {
                var val = new IndicatorValue
                {
                    Date = shortEma[i].Date,
                    Value = shortEma[i].Value - longEma[i - difference].Value
                };
                values.Add(val);
            }
            return values;
        }

        private static IList<IndicatorValue> PrepareResult(IList<IndicatorValue> macdLine, IList<IndicatorValue> signalLine)
        {
            IList<IndicatorValue> resultList = new List<IndicatorValue>();
            var difference = macdLine.Count - signalLine.Count;
            for (var i = difference; i < macdLine.Count; i++)
            {
                resultList.Add(new DoubleLineIndicatorValue
                {
                    Date = macdLine[i].Date,
                    Value = macdLine[i].Value,
                    SecondLineValue = signalLine[i - difference].Value
                });
            }
            return resultList;
        }

        public IList<Signal> GenerateSignals(IList<IndicatorValue> values)
        {
            var doubleLineValues = values.Cast<DoubleLineIndicatorValue>().ToList();
            var signals = new List<Signal>();
            var previousValue = doubleLineValues[0];
            for (int i = 1; i < doubleLineValues.Count; i++)
            {
                // we consider 2 lines - indicator and signal line
                // with equations 
                // y = (curr.* - prev.*)x + prev.* (y = Ax+B and y=Cx+D) - * may be Value or SecondLineValue
                // intersection exists when their difference
                // has value 0 somewhere, thus (C-A)x=B-D -> a=C-A, b=B-D
                // intersection exists if a=b=0 (lines overlap -> NOSIGNAL) or (x=B/A and 0<x<1)
                // if MACD intersects signal line upside -> BUY otherwise SELL
                var currentValue = doubleLineValues[i];
                decimal a = currentValue.SecondLineValue - previousValue.SecondLineValue - currentValue.Value +
                            previousValue.Value;
                decimal b = previousValue.Value - currentValue.Value;
                if ((a == 0 && b == 0) || (a*b > 0 && b < a))   // intersection
                {
                    decimal diff = previousValue.Value - previousValue.SecondLineValue;     // B - D
                    SignalAction action = ( diff== 0) ? SignalAction.NoSignal : (diff < 0 ? SignalAction.Buy : SignalAction.Sell);
                    var signal = new Signal(action)
                    {
                        Date = currentValue.Date
                    };
                    signals.Add(signal);
                }
                previousValue = currentValue;
            }
            return signals;
        }
    }
}
