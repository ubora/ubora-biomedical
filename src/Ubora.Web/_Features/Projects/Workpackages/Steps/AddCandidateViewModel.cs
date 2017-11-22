using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class AddCandidateViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Name { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Description { get; set; }
        [IsImage]
        [FileSize(4000000)]
        public IFormFile Image { get; set; }
    }
}