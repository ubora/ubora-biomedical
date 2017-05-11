using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Users.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
