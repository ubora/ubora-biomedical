using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Ubora.Web.Authorization;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class ProjectControllerRequirementHandlerTests
    {
        private readonly ProjectControllerRequirement.Handler _handlerUnderTest;

        public ProjectControllerRequirementHandlerTests()
        {
            _handlerUnderTest = new ProjectControllerRequirement.Handler();
        }

        [Fact]
        public async Task Handler_Allows_Everyone_Pass_When_Disabling_Filter_Is_Present_On_Controller_Action()
        {
            var filters = new IFilterMetadata[]
            {
                Mock.Of<IDisablesProjectControllerAuthorizationFilter>()
            }.ToList();

            var filterContext = new AuthorizationFilterContext(
                actionContext: new EmptyInitializedActionContext(),
                filters: filters);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new ProjectControllerRequirement() },
                user: null,
                resource: filterContext);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handler_Denies_Pass_When_User_Does_Not_Pass_Requirements(
            bool isAuthorized)
        {
            var httpContextMock = new Mock<HttpContext>();
            var actionContext = new EmptyInitializedActionContext
            {
                HttpContext = httpContextMock.Object,
            };

            var filterContext = new AuthorizationFilterContext(
                actionContext: actionContext,
                filters: new List<IFilterMetadata>());

            var currentUser = FakeClaimsPrincipalFactory.CreateAnonymousUser();
            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new ProjectControllerRequirement() },
                user: currentUser,
                resource: filterContext);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            httpContextMock
                .Setup(ctx => ctx.RequestServices.GetService(typeof(IAuthorizationService)))
                .Returns(authorizationServiceMock.Object);

            IAuthorizationRequirement[] authorizedRequirements = null;

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(currentUser, null, It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Callback<ClaimsPrincipal, object, IEnumerable<IAuthorizationRequirement>>(
                    (user, resource, requirements) => authorizedRequirements = requirements.ToArray())
                .ReturnsAsync(isAuthorized ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().Be(isAuthorized);

            authorizedRequirements.Length
                .Should().Be(2);

            authorizedRequirements
                .Should().Contain(x => x.GetType() == typeof(DenyAnonymousAuthorizationRequirement))
                .And.Contain(x => x.GetType() == typeof(IsProjectMemberRequirement));
        }
    }
}