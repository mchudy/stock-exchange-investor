﻿using StockExchange.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models
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
        public DateTime EndDate { get; set; }

        public IList<CompanyDto> Companies { get; set; }
        public IList<int> SelectedCompanyIds { get; set; }

        [Display(Name = "Strategy")]
        public IList<StrategyDto> Strategies { get; set; } = new List<StrategyDto>();
        public int SelectedStrategyId { get; set; }
    }
}