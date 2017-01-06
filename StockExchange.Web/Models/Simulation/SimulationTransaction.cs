using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Indicators;
using StockExchange.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Simulation
{
    public class SimulationTransaction
    {
        [DisplayFormat(DataFormatString = Consts.Formats.DisplayDate)]
        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Company name")]
        public CompanyDto Company { get; set; }

        [Display(Name = "Budget after")]
        public decimal BudgetAfter { get; set; }

        public SignalAction Action { get; set; }
    }
}
