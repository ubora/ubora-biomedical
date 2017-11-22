using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class EditProjectImageViewModel
    {
        [Required(ErrorMessage = "Please select an image to upload first!")]
        [IsImage]
        [FileSize(4000000, "Max project image size is 4 MB!")]
        public IFormFile Image { get; set; }
    }
}
