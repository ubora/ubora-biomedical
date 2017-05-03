using System;
using System.Security.Claims;
using Ubora.Web.Models;

namespace Ubora.Web.Services
{
    public static class PrincipalExtensions
    {
        public static string GetFullName(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ApplicationUser.FullNameClaimType);
        }

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));
            var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return new Guid(userIdString);
        }
    }
}
