using System;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Indicators
{
    public class SignalEvent
    {
        public DateTime Date { get; set; }
        
        public IList<int> CompaniesToSell { get; set; }

        public IList<int> CompaniesToBuy { get; set; }
    }
}
