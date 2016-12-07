using StockExchange.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models
{
    public class StrategyViewModel
    {
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public IList<CompanyDto> Companies { get; set; }
        public IList<int> SelectedCompanyIds { get; set; }
    }
}