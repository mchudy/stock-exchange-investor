using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("SimulationCompany")]
    public class SimultionCompany
    {
        public decimal Weight { get; set; }

        public virtual Company Company { get; set; }

        public virtual Simulation Simulation { get; set; }
    }
}
