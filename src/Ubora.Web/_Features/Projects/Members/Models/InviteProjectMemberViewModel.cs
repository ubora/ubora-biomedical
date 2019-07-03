using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Members.Models
{
    public class InviteProjectMemberViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}