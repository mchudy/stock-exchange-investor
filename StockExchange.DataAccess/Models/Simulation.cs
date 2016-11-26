using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("Simulation")]
    public class Simulation
    {
        public int Id { get; set;}
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual InvestmentStrategy Strategy { get; set; }

        public virtual ICollection<SimultionCompany> SimultionCompanies { get; set; } = new HashSet<SimultionCompany>();
    }
}
