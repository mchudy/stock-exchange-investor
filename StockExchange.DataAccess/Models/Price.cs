using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// Represents a stock price
    /// </summary>
    public class Price
    {
        /// <summary>
        /// The price ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The company ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// The date
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// The open price
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// The close price
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// The maximum price of stocks during the day
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// The minimum price of stocks during the day
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// The number of transactions during the day
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// The company
        /// </summary>
        //TODO: this should't be here
        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
