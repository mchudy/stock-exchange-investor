using System.Collections.Generic;
using System.Linq;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Wskaźnik Macd
    /// </summary>
    public class MacdIndicator : IIndicator
    {
        public int LongTerm { get; set; }

        public int ShortTerm { get; set; }

        public int SignalTerm { get; set; }

        public MacdIndicator()
        {
            Initialize();
        }

        private void Initialize()
        {
            LongTerm = 26;
            ShortTerm = 12;
            SignalTerm = 9;
        }

        public IList<IndicatorValue> Calculate(IList<Price> historicalPrices)
        {
            var longEma = MovingAverageHelper.ExpotentialMovingAverage(historicalPrices, LongTerm);
            var shortEma = MovingAverageHelper.ExpotentialMovingAverage(historicalPrices, ShortTerm);
            var macdLine = SubstractLongEmaFromShortEma(shortEma, longEma);
            var signalLine = MovingAverageHelper.ExpotentialMovingAverage(macdLine, SignalTerm);
            return PrepareResult(macdLine, signalLine);
        }

        private IList<IndicatorValue> SubstractLongEmaFromShortEma(IList<IndicatorValue> shortEma, IList<IndicatorValue> longEma)
        {
            int difference = LongTerm - ShortTerm;
            IList<IndicatorValue> values=new List<IndicatorValue>();
            for (int i = difference; i < shortEma.Count; i++)
            {
                var val = new IndicatorValue()
                {
                    Date = shortEma[i].Date,
                    Value = shortEma[i].Value - longEma[i-difference].Value
                };
                values.Add(val);
            }
            return values;
        }

        private IList<IndicatorValue> PrepareResult(IList<IndicatorValue> macdLine, IList<IndicatorValue> signalLine)
        {
            IList<IndicatorValue> resultList = new List<IndicatorValue>();
            int difference = macdLine.Count - signalLine.Count;
            for (int i = difference; i < macdLine.Count; i++)
            {
                resultList.Add(new DoubleLineIndicatorValue()
                {
                    Date = macdLine[i].Date,
                    Value = macdLine[i].Value,
                    SecondLineValue = signalLine[i-difference].Value
                });
            }
            return resultList;
        }
    }
}
