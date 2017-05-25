using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsAuthenticatedUserAuthorizationHandlerTests
    {
        private readonly IsAuthenticatedUserAuthorizationHandler _handlerUnderTest;

        public IsAuthenticatedUserAuthorizationHandlerTests()
        {
            _handlerUnderTest = new IsAuthenticatedUserAuthorizationHandler();
        }

        [Fact]
        public async Task Handler_Succeeds_Only_When_User_Is_Authenticated()
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(Enumerable.Empty<Claim>(), authenticationType: "any"));

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsAuthenticatedUserRequirement() },
                user: authenticatedUser,
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handler_Does_Nothing_When_User_Is_Not_Authenticated()
        {
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsAuthenticatedUserRequirement() },
                user: new ClaimsPrincipal(), 
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeFalse();
        }
    }
}
