using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Features.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
