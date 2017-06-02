using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class CanRemoveProjectMemberAuthorizationHandlerTests
    {
        private readonly CanRemoveProjectMemberAuthorizationHandler _handlerUnderTest;

        public CanRemoveProjectMemberAuthorizationHandlerTests()
        {
            _handlerUnderTest = new CanRemoveProjectMemberAuthorizationHandler();
        }

        [Fact]
        public async Task Handler_Denies_Access_When_Project_Is_Not_Found()
        {
            var httpContext = Mock.Of<HttpContext>(x => x.RequestServices.GetService(typeof(IQueryProcessor)) == Mock.Of<IQueryProcessor>());

            var filterContext = new AuthorizationFilterContext(
                actionContext: new EmptyInitializedActionContext
                {
                    HttpContext = httpContext
                },
                filters: new List<IFilterMetadata>());

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new CanRemoveProjectMemberRequirement() },
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
                requirements: new[] { new CanRemoveProjectMemberRequirement() },
                user: ClaimsPrincipal.Current,
                resource: filterContext);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handler_Denies_Pass_When_User_Is_Not_Authenticated()
        {
            var routeData = new RouteData();
            var projectId = Guid.NewGuid();
            routeData.Values.Add("projectId", projectId.ToString());

            var httpContextMock = new Mock<HttpContext>();
            var actionContext = new EmptyInitializedActionContext
            {
                HttpContext = httpContextMock.Object,
                RouteData = routeData
            };

            var filterContext = new AuthorizationFilterContext(
                actionContext: actionContext,
                filters: new List<IFilterMetadata>());

            //var userId = Guid.NewGuid();
            //var claims = new Claim[] { new Claim(ClaimTypes.Anonymous, userId.ToString()) };
            var authenticatedUser = new ClaimsPrincipal();

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new CanRemoveProjectMemberRequirement() },
                user: authenticatedUser,
                resource: filterContext);

            var queryProcessorMock = new Mock<IQueryProcessor>();

            httpContextMock
                .Setup(ctx => ctx.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(queryProcessorMock.Object);

            var project = Mock.Of<Project>();
            queryProcessorMock
                .Setup(x => x.FindById<Project>(projectId))
                .Returns(project);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasFailed.Should().BeTrue();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Handler_Succeeds_Only_When_User_Is_Leader_In_Given_Project(bool isMember)
        {
            var routeData = new RouteData();
            var projectId = Guid.NewGuid();
            routeData.Values.Add("projectId", projectId.ToString());

            var httpContextMock = new Mock<HttpContext>();
            var actionContext = new EmptyInitializedActionContext
            {
                HttpContext = httpContextMock.Object,
                RouteData = routeData
            };

            var filterContext = new AuthorizationFilterContext(
                actionContext: actionContext,
                filters: new List<IFilterMetadata>());

            var userId = Guid.NewGuid();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "any"));

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new CanRemoveProjectMemberRequirement() },
                user: authenticatedUser,
                resource: filterContext);

            var queryProcessorMock = new Mock<IQueryProcessor>();

            httpContextMock
                .Setup(ctx => ctx.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(queryProcessorMock.Object);

            var project = Mock.Of<Project>(x => x.DoesSatisfy(new IsLeader(userId)) == isMember);

            queryProcessorMock
                .Setup(x => x.FindById<Project>(projectId))
                .Returns(project);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().Be(isMember);
        }
    }
}
