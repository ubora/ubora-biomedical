using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class EditCandidateViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
