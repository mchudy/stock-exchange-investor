using System;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// Represents a transaction concluded by a user
    /// </summary>
    public class UserTransaction
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date of the transaction
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Price of a single stock
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Number of stocks bought or sold (negative if sold, positive if bought)
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Company
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// Company ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }
    }
}
