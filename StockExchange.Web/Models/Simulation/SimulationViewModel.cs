using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Strategy;
using StockExchange.Web.Helpers.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Simulation
{
    /// <summary>
    /// View model for a trading simulation
    /// </summary>
    public class SimulationViewModel
    {
        /// <summary>
        /// Start date
        /// </summary>
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DateCompare(nameof(StartDate), DateTimeComparer.IsLessThanOrEqualTo, 
            ErrorMessage = "End date must be later than the start date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Companies included in the simulation
        /// </summary>
        public IList<CompanyDto> Companies { get; set; }

        /// <summary>
        /// Ids of companies to include in the simulation
        /// </summary>
        public IList<int> SelectedCompanyIds { get; set; }

        /// <summary>
        /// All companies available
        /// </summary>
        [RequiredIf(nameof(AllCompanies), false)]
        [Display(Name = "All companies")]
        public bool AllCompanies { get; set; }

        /// <summary>
        /// List of all company groups
        /// </summary>
        [Display(Name = "Company group")]
        public IList<CompanyGroupDto> CompanyGroups { get; set; }

        /// <summary>
        /// All strategies available
        /// </summary>
        [Display(Name = "Strategy")]
        public IList<StrategyDto> Strategies { get; set; } = new List<StrategyDto>();

        /// <summary>
        /// Id of the selected strategy
        /// </summary>
        [Required]
        public int SelectedStrategyId { get; set; }

        /// <summary>
        /// Total budget for the simulation
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Budget must be greater than 0")]
        public decimal Budget { get; set; } = 10000;

        /// <summary>
        /// Budget limit for a single buy transaction
        /// </summary>
        [Display(Name = "Transaction limit value")]
        public decimal MaximalBudgetPerTransaction { get; set; }

        /// <summary>
        /// Indicates whether transaction limit is defined
        /// </summary>
        [Display(Name = "Transaction limit")]
        public bool HasTransactionLimit { get; set; }
    }
}