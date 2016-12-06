using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockExchange.Business.Models;

namespace StockExchange.Web.Models
{
    public class UpdateStrategyViewModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public IList<int> Companies { get; set; }
    }
}