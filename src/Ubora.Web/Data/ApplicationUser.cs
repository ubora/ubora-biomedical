using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ubora.Web.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public static readonly string ProfilePictureUrlClaimType = "Ubora.UserProfile.ProfilePictureUrl";
        public static readonly string FullNameClaimType = "Ubora.UserProfile.FullName";
    }
}
