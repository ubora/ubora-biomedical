using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class EditProfileViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        [IsImage]
        public IFormFile ProfilePicture { get; set; }
    }
}