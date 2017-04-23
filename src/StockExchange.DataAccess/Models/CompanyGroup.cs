using System.Collections.Generic;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// A company group
    /// </summary>
    public class CompanyGroup
    {
        /// <summary>
        /// The company group ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The company group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Companies in the company group
        /// </summary>
        public virtual ICollection<CompanyGroupCompany> CompanyGroupCompanies { get; set; } 
            = new HashSet<CompanyGroupCompany>();
    }
}
