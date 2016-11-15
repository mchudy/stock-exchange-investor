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
