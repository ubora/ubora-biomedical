using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ubora.Web.Tests.Fakes
{
    public static class FakeClaimsPrincipalFactory
    {
        public static ClaimsPrincipal CreateAuthenticatedUser(Guid? userId)
        {
            var claims = new List<Claim>();

            if (userId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            return user;
        }
    }
}