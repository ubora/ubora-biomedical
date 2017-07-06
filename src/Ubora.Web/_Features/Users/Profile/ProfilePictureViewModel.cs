using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfilePictureViewModel
    {
        [IsImage]
        public IFormFile ProfilePicture { get; set; }

        public bool IsFirstTimeEditProfile { get; set; }
    }
}
