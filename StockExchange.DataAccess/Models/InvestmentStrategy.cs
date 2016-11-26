using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("Strategy")]
    public class InvestmentStrategy
    {
        public int Id { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<InvestmentCondition> Conditions { get; set; } = new HashSet<InvestmentCondition>();
    }
}
