using System;

namespace StockExchange.Business.Models
{
    public class SimulationValue
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public SimulationResult SimulationResult { get; set; }
    }
}
