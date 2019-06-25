using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
