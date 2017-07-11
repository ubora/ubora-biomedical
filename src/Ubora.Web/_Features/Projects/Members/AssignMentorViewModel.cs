using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Members
{
    public class AssignMentorViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}