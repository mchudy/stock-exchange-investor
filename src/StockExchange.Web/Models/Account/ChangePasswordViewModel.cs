using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Account
{
    /// <summary>
    /// View model for changing password form
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Old password
        /// </summary>
        [Required]
        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm new password
        /// </summary>
        [Required]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

    }
}