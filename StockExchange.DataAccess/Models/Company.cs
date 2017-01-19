using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// A company
    /// </summary>
    public class Company
    {
        /// <summary>
        /// The company ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The company code
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        /// <summary>
        /// The company name
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Stock prices for the company
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Price> Prices { get; set; } = new HashSet<Price>();

        public virtual ICollection<CompanyGroupCompany> CompanyGroupCompanies { get; set; } = 
            new List<CompanyGroupCompany>();
    }
}
