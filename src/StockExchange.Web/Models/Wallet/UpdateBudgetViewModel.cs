using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Wallet
{
    /// <summary>
    /// View model for updating the free budget value
    /// </summary>
    public class UpdateBudgetViewModel
    {
        /// <summary>
        /// New budget
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "New budget")]
        public decimal NewBudget { get; set; }
    }
}