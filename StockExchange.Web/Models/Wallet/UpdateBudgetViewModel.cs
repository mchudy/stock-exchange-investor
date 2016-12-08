using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Wallet
{
    public class UpdateBudgetViewModel
    {
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "New budget")]
        public decimal NewBudget { get; set; }
    }
}