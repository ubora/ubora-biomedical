using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Manage
{
    public class ChangeProfilePictureViewModel
    {
        [Required]
        [IsImage]
        public IFormFile ProfilePicture { get; set; }
    }
}
