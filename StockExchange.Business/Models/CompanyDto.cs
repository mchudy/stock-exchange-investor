using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models
{
    public sealed class CompanyDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Company Name")]
        public string Name { get; set; }
    }
}
