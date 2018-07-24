using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ubora.Web.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public static readonly string ProfilePictureUrlClaimType = "Ubora.UserProfile.ProfilePictureUrl";
        public static readonly string FullNameClaimType = "Ubora.UserProfile.FullName";
        public static readonly string FirstNameClaimType = "Ubora.UserProfile.FirstName";
        public static readonly string EmailClaimType = "Ubora.UserProfile.Email";
        public static readonly string IsEmailConfirmedType = "Ubora.UserManager.IsEmailConfirmed";
    }
}
