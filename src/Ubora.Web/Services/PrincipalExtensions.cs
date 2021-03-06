﻿using System;
using System.Security.Claims;
using System.Security.Principal;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web.Data;

namespace Ubora.Web.Services
{
    public static class PrincipalExtensions
    {
        public static string GetFullName(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal)principal;
            var fullName = claimsPrincipal.FindFirstValue(ApplicationUser.FullNameClaimType);

            return fullName;
        }

        public static string GetFirstName(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal)principal;
            var fullName = claimsPrincipal.FindFirstValue(ApplicationUser.FirstNameClaimType);

            return fullName;
        }

        public static string GetProfilePictureUrl(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal)principal;
            var profilePictureUrl = claimsPrincipal.FindFirstValue(ApplicationUser.ProfilePictureUrlClaimType);

            return profilePictureUrl;
        }

        public static Guid GetId(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal) principal;
            var userIdString = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = new Guid(userIdString);

            return userId;
        }

        public static string GetEmail(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal)principal;
            var userEmail = claimsPrincipal.FindFirstValue(ApplicationUser.EmailClaimType);

            return userEmail;
        }

        public static bool IsEmailConfirmed(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsPrincipal = (ClaimsPrincipal)principal;
            var isEmailConfirmed = Convert.ToBoolean(claimsPrincipal.FindFirstValue(ApplicationUser.IsEmailConfirmedType));

            return isEmailConfirmed;
        }

        public static UserInfo GetInfo(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var info = new UserInfo(principal.GetId(), principal.GetFullName());

            return info;
        }
    }
}
