using System;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents an extreme simulation value
    /// </summary>
    public class ExtremeSimulationValue
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExtremeSimulationValue"/>
        /// </summary>
        /// <param name="date">Date when the extremum occurred</param>
        /// <param name="value">Value of stocks at that day</param>
        /// <param name="startBudget">The start budget of the simulation</param>
        public ExtremeSimulationValue(DateTime date, decimal value, decimal startBudget)
        {
            Date = date;
            Value = value;
            PercentageIncome = Math.Round((double)((value - startBudget) / startBudget), 2) * 100;
        }

        /// <summary>
        /// Date when the extremum occurred
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Value of stocks at that day
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// The percentage income
        /// </summary>
        public double PercentageIncome { get; set; }
    }
}