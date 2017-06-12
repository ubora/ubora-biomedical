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
            var claims = new List<Claim>();

            if (userId != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
            }

            if (fullName != null)
            {
                claims.Add(new Claim(ApplicationUser.FullNameClaimType, fullName));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            return user;
        }
    }
}