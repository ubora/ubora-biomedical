using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsUboraManagementGroupGenericRequirementHandlerTests
    {
        private readonly IsUboraManagementGroupGenericRequirementHandler<DummyAuthorizationRequirement> _handlerUnderTest;

        public IsUboraManagementGroupGenericRequirementHandlerTests()
        {
            _handlerUnderTest = new IsUboraManagementGroupGenericRequirementHandler<DummyAuthorizationRequirement>();
        }

        [Fact]
        public async Task Handler_Succeeds_For_User_With_Management_Group_Role()
        {
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new DummyAuthorizationRequirement() },
                user: FakeClaimsPrincipalFactory.CreateAuthenticatedUser(roleNames: new[] { ApplicationRole.ManagementGroup }),
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Handler_Does_Not_Succeed_When_User_Does_Not_Have_Management_Group_Role()
        {
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new DummyAuthorizationRequirement() },
                user: FakeClaimsPrincipalFactory.CreateAuthenticatedUser(),
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }
    }
}
