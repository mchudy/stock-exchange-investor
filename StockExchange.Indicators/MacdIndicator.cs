using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Indicators.Helpers;
using StockExchange.Indicators.Lines;

namespace StockExchange.Indicators
{
    public class MacdIndicator
    {
        public int LongTerm { get; }
        public int ShortTerm { get; }
        public int SignalTerm { get; }

        public MacdIndicator(int longTerm = 26, int shortTerm = 12, int signalTerm=9)
        {
            if(longTerm<= shortTerm)
                throw new ArgumentException();
            LongTerm = longTerm;
            ShortTerm = shortTerm;
            SignalTerm = signalTerm;
        }

        private PointLine CalculateMacdLine(IEnumerable<decimal> historicalData)
        {
            return MacdHelper.CalculateMacdLine(historicalData, LongTerm, ShortTerm);
        }

        private PointLine CalculateSignalLine(IEnumerable<decimal> historicalData)
        {
            return MacdHelper.CalculateSignalLine(CalculateMacdLine(historicalData), SignalTerm);
        }

        public IEnumerable<SignalEventAction> Simulate(IEnumerable<decimal> historicalData)
        {
            var macd = CalculateMacdLine(historicalData);
            var signal = MacdHelper.CalculateSignalLine(macd, SignalTerm);
            PointLine macd1 = new PointLine(macd.Values.Skip(SignalTerm));
            return PointLineInterceptionHelper.FindAllInterceptions(macd1, signal).Select(Convert).Where(s=>s!=SignalEventAction.None);
        }

        private SignalEventAction Convert(InterceptionInfo info)
        {
            if(info.StartValue1 < info.StatrValue2 && info.EndValue1 > info.StatrValue2)
                return SignalEventAction.Buy;
            if(info.StartValue1>info.StatrValue2 && info.EndValue1<info.EndValue2)
                return SignalEventAction.Sell;
            return SignalEventAction.None;
        }
    }
}
