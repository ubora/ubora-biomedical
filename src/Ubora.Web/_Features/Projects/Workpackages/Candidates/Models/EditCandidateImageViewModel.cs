using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    public class EditCandidateImageViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please select an image to upload first!")]
        [IsImage]
        [FileSize(4000000)]
        public IFormFile Image { get; set; }
    }
}