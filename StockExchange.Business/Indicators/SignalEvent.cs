using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators
{
    public enum SignalEventAction
    {
        Buy, 
        Sell,
        None
    }

    public class SignalEvent
    {
        public SignalEventAction Action { get; }

        public SignalEvent(SignalEventAction action)
        {
            Action = action;
        }
    }
}
