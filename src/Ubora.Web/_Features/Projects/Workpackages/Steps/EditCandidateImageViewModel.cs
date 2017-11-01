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
        [Required]
        public IFormFile Image { get; set; }
    }
}