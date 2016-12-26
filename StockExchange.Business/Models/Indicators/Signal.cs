using System;

namespace StockExchange.Business.Models.Indicators
{
    public enum SignalAction
    {
        Buy,
        Sell,
        NoSignal
    }

    public class Signal
    {
        public SignalAction Action { get; set; }

        public DateTime Date { get; set; }

        public Signal(SignalAction action)
        {
            Action = action;
        }
    }
}
