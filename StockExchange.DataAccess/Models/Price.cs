using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("Price")]
    public sealed class Price
    {
        public int id { get; set; }

        public int companyId { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public decimal openPrice { get; set; }

        public decimal closePrice { get; set; }

        public decimal highPrice { get; set; }

        public decimal lowPrice { get; set; }

        public int volume { get; set; }

        public Company Company { get; set; }
    }
}
