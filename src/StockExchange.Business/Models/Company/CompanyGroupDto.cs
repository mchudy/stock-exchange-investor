using System.Collections.Generic;

namespace StockExchange.Business.Models.Company
{
    /// <summary>
    /// DTO object for a company group
    /// </summary>
    public class CompanyGroupDto
    {
        /// <summary>
        /// Company group ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IDs of companies which the group contains
        /// </summary>
        public List<int> CompanyIds { get; set; } = new List<int>();
    }
}
