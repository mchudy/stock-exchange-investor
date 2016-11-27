using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    [Table("Wallet")]
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Budget { get; set; }
        public virtual ICollection<UserTransaction> Transactions { get; set; } = new HashSet<UserTransaction>();
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
