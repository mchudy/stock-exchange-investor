using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.DataAccess.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Price> Prices { get; set; } = new HashSet<Price>();
    }
}
