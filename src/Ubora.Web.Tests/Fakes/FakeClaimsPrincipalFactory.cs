using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ubora.Web.Data;

namespace Ubora.Web.Tests.Fakes
{
    public static class FakeClaimsPrincipalFactory
    {
        public static ClaimsPrincipal CreateAuthenticatedUser(
            Guid? userId = null,
            string fullName = null,
            bool isEmailConfirmed = false,
            IEnumerable<string> roleNames = null)
        {
            var claims = CreateUserClaims(userId, fullName, roleNames ?? Enumerable.Empty<string>());
            if(isEmailConfirmed)
            {
                claims.Add(new Claim(ApplicationUser.IsEmailConfirmedType, "true"));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            return user;
        }

        public static ClaimsPrincipal CreateAnonymousUser()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        private static List<Claim> CreateUserClaims(Guid? userId, string fullName, IEnumerable<string> roleNames)
        {
            var claims = new List<Claim>();

            if (userId != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
            }

            if (fullName != null)
            {
                claims.Add(new Claim(ApplicationUser.FullNameClaimType, fullName));
            }

            if (roleNames.Any())
            {
                roleNames.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            }

            return claims;
        }
    }
}