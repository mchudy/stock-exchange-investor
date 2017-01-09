using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Indicators;
using StockExchange.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Simulation
{
    /// <summary>
    /// Represents a transaction concluded during the simulation
    /// </summary>
    public class SimulationTransaction
    {
        /// <summary>
        /// The date of the transaction
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.DisplayDate)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of stocks involved
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The company id
        /// </summary>
        [Display(Name = "Company name")]
        public CompanyDto Company { get; set; }

        /// <summary>
        /// The total budget after concluding the transaction
        /// </summary>
        [Display(Name = "Budget after")]
        public decimal BudgetAfter { get; set; }

        /// <summary>
        /// The action taken
        /// </summary>
        public SignalAction Action { get; set; }
    }
}
