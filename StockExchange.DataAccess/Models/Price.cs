using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("Price")]
    public sealed class Price
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal HighPrice { get; set; }

        public decimal LowPrice { get; set; }

        public int Volume { get; set; }

        public Company Company { get; set; }
    }
}
