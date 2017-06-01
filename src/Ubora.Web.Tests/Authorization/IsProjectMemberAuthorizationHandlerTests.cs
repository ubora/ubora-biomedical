using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsProjectMemberAuthorizationHandlerTests
    {
        private readonly IsProjectMemberAuthorizationHandler _handlerUnderTest;

        private readonly Mock<IIsMemberPartOfProject> _isMemberPartOfProject;

        public IsProjectMemberAuthorizationHandlerTests()
        {
            _isMemberPartOfProject = new Mock<IIsMemberPartOfProject>();
            _handlerUnderTest = new IsProjectMemberAuthorizationHandler(_isMemberPartOfProject.Object);
        }

        [Fact]
        public async Task Handler_Denies_Access_When_Project_Id_Can_Not_Be_Parsed_From_Route()
        {
            var filterContext = new AuthorizationFilterContext(
                actionContext: new EmptyInitializedActionContext(),
                filters: new List<IFilterMetadata>());

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectMemberRequirement() },
                user: ClaimsPrincipal.Current,
                resource: filterContext);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handler_Allows_Everyone_Pass_When_Disabling_Filter_Is_Present_On_Action()
        {
            var filters = new IFilterMetadata[]
            {
                Mock.Of<IDisablesProjectAuthorizationPolicyFilter>()
            }.ToList();

            var filterContext = new AuthorizationFilterContext(
                actionContext: new EmptyInitializedActionContext(),
                filters: filters);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectMemberRequirement() },
                user: ClaimsPrincipal.Current,
                resource: filterContext);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handler_Succeeds_Only_When_User_Is_Member_In_Given_Project(bool isMember)
        {
            var routeData = new RouteData();
            var projectId = Guid.NewGuid();
            routeData.Values.Add("projectId", projectId.ToString());
            var actionContext = new EmptyInitializedActionContext
            {
                RouteData = routeData
            };

            var filterContext = new AuthorizationFilterContext(
                actionContext: actionContext,
                filters: new List<IFilterMetadata>());

            var userId = Guid.NewGuid();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectMemberRequirement() },
                user: authenticatedUser,
                resource: filterContext);

            _isMemberPartOfProject.Setup(x => x.Satisfy(projectId, userId)).Returns(isMember);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().Be(isMember);
        }
    }
}
