using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfilePictureViewModel
    {
        public string CurrentActionName { get; set; }
        [IsImage]
        public IFormFile ProfilePicture { get; set; }
    }
}
