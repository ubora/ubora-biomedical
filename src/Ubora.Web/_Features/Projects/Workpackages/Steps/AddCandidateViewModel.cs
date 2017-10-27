using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class AddCandidateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [IsImage]
        [FileSize(4000000)]
        public IFormFile Image { get; set; }
    }
}