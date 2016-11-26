using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("CompanyStockQuantity")]
    public class CompanyStockQuantity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual Company Company { get; set; }
        public virtual SimulationResult SimulationResult { get; set; }
    }
}
