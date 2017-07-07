using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfilePictureViewModel
    {
        [IsImage]
        [FileSize(1000000, "The limit for profile image is 1 MB!")]
        public IFormFile ProfilePicture { get; set; }

        public bool IsFirstTimeEditProfile { get; set; }
    }
}
