using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
