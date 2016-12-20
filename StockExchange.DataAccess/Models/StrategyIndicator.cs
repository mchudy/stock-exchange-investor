using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("StrategyIndicator")]
    public class StrategyIndicator
    {
        public int Id { get; set; }
        public int IndicatorType { get; set; }
        public virtual InvestmentStrategy Strategy { get; set; }
        public int StrategyId { get; set; }
        public virtual ICollection<StrategyIndicatorProperty> Properties { get; set; } = new HashSet<StrategyIndicatorProperty>(); 
    }
}
