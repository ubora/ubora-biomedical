using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Members
{
    public class InviteProjectMemberViewModel
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Email { get; set; }
    }
}