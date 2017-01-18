using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// Entity representing a company in a company group
    /// </summary>
    public class CompanyGroupCompany
    {
        /// <summary>
        /// The company ID
        /// </summary>
        [Key, Column(Order = 1)]
        public int CompanyId { get; set; }

        /// <summary>
        /// The company group ID
        /// </summary>
        [Key, Column(Order = 2)]
        public int CompanyGroupId { get; set; }

        /// <summary>
        /// The company
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// The company group
        /// </summary>
        public virtual CompanyGroup CompanyGroup { get; set; }
    }
}