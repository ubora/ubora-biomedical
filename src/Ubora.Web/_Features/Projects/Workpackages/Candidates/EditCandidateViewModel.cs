using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    public class EditCandidateViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Description { get; set; }
    }
}
