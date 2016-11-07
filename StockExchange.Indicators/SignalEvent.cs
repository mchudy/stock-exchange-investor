using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Indicators
{
    public enum SignalEventAction
    {
        Buy,
        Sell,
        None
    }

    public class SignalEvent
    {
        public int Index { get; }
        public SignalEventAction Action { get; }

        public SignalEvent(int index, SignalEventAction action)
        {
            Index = index;
            Action = action;
        }
    }
}
