using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("SimulationResult")]
    public class SimulationResult
    {
        public int Id { get; set; }

        public virtual Simulation Simulation { get; set; }

        public virtual ICollection<SimulationValue> SimulationValues { get; set; } = new HashSet<SimulationValue>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

        public virtual ICollection<CompanyStockQuantity> CompanyStockQuantities { get; set; } = new HashSet<CompanyStockQuantity>();
    }
}
