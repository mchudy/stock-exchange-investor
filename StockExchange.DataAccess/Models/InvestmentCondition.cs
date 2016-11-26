using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("Condition")]
    public class InvestmentCondition
    {
        public string IndicatorName { get; set; }

        public decimal? BuyValue { get; set; }
        public decimal? SellValue { get; set; }

        public virtual InvestmentStrategy Strategy { get; set; }
    }
}
