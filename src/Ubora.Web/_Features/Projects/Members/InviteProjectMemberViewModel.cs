using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Members
{
    public class InviteProjectMemberViewModel
    {
        public Guid ProjectId { get; set; }

        [Required]
        public Guid? UserId { get; set; }
    }
}