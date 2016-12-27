using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Strategy;
using StockExchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Simulation
{
    public class SimulationViewModel
    {
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DateCompare(nameof(StartDate), DateTimeComparer.IsLessThanOrEqualTo, 
            ErrorMessage = "End date must be later than the start date")]
        public DateTime EndDate { get; set; }

        public IList<CompanyDto> Companies { get; set; }

        [Required]
        public IList<int> SelectedCompanyIds { get; set; }

        [Display(Name = "Strategy")]
        public IList<StrategyDto> Strategies { get; set; } = new List<StrategyDto>();

        [Required]
        public int SelectedStrategyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Budget must be greater than 0")]
        public decimal Budget { get; set; }
    }
}