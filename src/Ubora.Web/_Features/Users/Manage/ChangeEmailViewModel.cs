using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Users.Manage
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string NewEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
