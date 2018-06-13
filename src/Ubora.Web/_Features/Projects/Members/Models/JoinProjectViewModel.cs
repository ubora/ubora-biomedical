using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Members.Models
{
    public class JoinProjectViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
