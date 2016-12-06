using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StockExchange.Business.Models;

namespace StockExchange.Web.Models
{
    public class StrategyViewModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public IList<CompanyDto> Companies { get; set; }
    }
}