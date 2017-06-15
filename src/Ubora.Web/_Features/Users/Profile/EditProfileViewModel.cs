using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class EditProfileViewModel
    {
        public UserProfileViewModel UserViewModel { get; set; }
        [IsImage]
        public IFormFile ProfilePicture { get; set; }
    }
}