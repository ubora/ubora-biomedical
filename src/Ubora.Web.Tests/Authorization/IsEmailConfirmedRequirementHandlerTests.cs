using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Threading.Tasks;
using Ubora.Web.Authorization;
using Ubora.Web.Services;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsEmailConfirmedRequirementHandlerTests
    {
        private readonly HandlerUnderTest _handlerUnderTest;

        public IsEmailConfirmedRequirementHandlerTests()
        {
            _handlerUnderTest = new HandlerUnderTest();
        }

        [Fact]
        public async Task Succeeds_When_User_Email_Is_Confirmed()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: userId, isEmailConfirmed: true);
            var isPoop = user.IsEmailConfirmed();
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsEmailConfirmedRequirement() },
                user: user,
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_Not_Succeed_When_User_Email_Is_Not_Confirmed()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: userId, isEmailConfirmed: false);
            var isPoop = user.IsEmailConfirmed();
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsEmailConfirmedRequirement() },
                user: user,
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }

        private class HandlerUnderTest : IsEmailConfirmedRequirement.Handler
        {
        }
    }
}
