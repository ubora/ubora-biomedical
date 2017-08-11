using System;
using System.Collections.Generic;
using System.Security.Claims;
using Ubora.Web.Data;

namespace Ubora.Web.Tests.Fakes
{
    public static class FakeClaimsPrincipalFactory
    {
        public static ClaimsPrincipal CreateAuthenticatedUser(
            Guid? userId = null,
            string fullName = null)
        {
            var claims = CreateUserClaims(userId, fullName);

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            return user;
        }

        public static ClaimsPrincipal CreateAnonymousUser()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        public static ClaimsPrincipal CreateConfirmedUser(Guid? userId = null, string fullName = null)
        {
            var claims = CreateUserClaims(userId, fullName);
            claims.Add(new Claim(ApplicationUser.IsEmailConfirmedType, "true"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            return user;
        }

        private static List<Claim> CreateUserClaims(Guid? userId, string fullName)
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

            return claims;
        }
    }
}