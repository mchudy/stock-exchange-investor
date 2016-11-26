using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("SimulationCompany")]
    public class SimulationCompany
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }

        public virtual Company Company { get; set; }

        public virtual Simulation Simulation { get; set; }
    }
}
