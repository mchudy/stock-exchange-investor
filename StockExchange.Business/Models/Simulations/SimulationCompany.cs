using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Models.Simulations
{
    public class SimulationCompany
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int SimulationId { get; set; }
        public virtual Simulation Simulation { get; set; }
    }
}
