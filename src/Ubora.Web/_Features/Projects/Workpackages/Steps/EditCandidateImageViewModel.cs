using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class EditCandidateImageViewModel
    {
        public Guid Id { get; set; }

        [IsImage]
        [FileSize(4000000)]
        [Required(ErrorMessage = "Please select an image to upload first!")]
        public IFormFile Image { get; set; }
    }
}