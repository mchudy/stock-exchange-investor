using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Company
{
    /// <summary>
    /// Represent a company
    /// </summary>
    public sealed class CompanyDto
    {
        /// <summary>
        /// Company id
        /// </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Company code
        /// </summary>
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        [Display(Name = "Company Name")]
        public string Name { get; set; }
    }
}