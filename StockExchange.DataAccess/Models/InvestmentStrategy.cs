using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("Strategy")]
    public class InvestmentStrategy
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<StrategyIndicator> Indicators { get; set; } = new HashSet<StrategyIndicator>();
    }
}
