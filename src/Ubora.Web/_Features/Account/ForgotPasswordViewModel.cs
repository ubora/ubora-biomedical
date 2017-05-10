using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
