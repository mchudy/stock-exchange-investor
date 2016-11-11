using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Wskaźnik Macd
    /// </summary>
    public class MacdIndicator : BaseIndicator
    {
        public int LongTerm { get; set; }
        public int ShortTerm { get; set; }
        public int SignalTerm { get; set; }

        public MacdIndicator() : base()
        {
            Initialize();
        }

        public MacdIndicator(IRepository<Price> priceRepository) : base(priceRepository)
        {
            Initialize();
        }

        private void Initialize()
        {
            LongTerm = 26;
            ShortTerm = 12;
            SignalTerm = 9;
        }

        public override IList<decimal> Calculate(IList<Price> historicalPrices)
        {
            return CalculateMacdLine(historicalPrices);
        }

        public IList<decimal> CalculateMacdLine(IList<Price> historicalPrices)
        {
            var list = historicalPrices.Select(x => x.ClosePrice).ToList();
            var longEma = MovingAverageHelper.ExpotentialMovingAverage(list, LongTerm);
            var shortEma = MovingAverageHelper.ExpotentialMovingAverage(list, ShortTerm);
            int diff = LongTerm - ShortTerm;
            var macdLine = new List<decimal>();
            for (int i = 0; i < shortEma.Count; i++)
                macdLine.Add(shortEma[i] - longEma[i + diff]);
            return macdLine;
        }

        public IList<decimal> CalculateSignalLine(IList<Price> historicalPrices)
        {
            var list = Calculate(historicalPrices);
            return MovingAverageHelper.ExpotentialMovingAverage(list, SignalTerm);
        }

        private IList<decimal> CalculateSignalLine(IList<decimal> macdLine)
        {
            return MovingAverageHelper.ExpotentialMovingAverage(macdLine, SignalTerm);
        }
    }
}
