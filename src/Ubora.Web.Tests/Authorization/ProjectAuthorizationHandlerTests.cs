using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Xunit;
using Ubora.Web.Tests.Fakes;

namespace Ubora.Web.Tests.Authorization
{
    public class ProjectAuthorizationHandlerTests
    {
        [Fact]
        public async Task Does_Not_Handle_Requirements_When_Project_Is_Not_Found_From_Route()
        {
            var routeData = new RouteData();
            var routingFeature = new RoutingFeature { RouteData = routeData };

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock
                .Setup(x => x.Features[typeof(IRoutingFeature)])
                .Returns(routingFeature);
            httpContextMock
                .Setup(x => x.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(Mock.Of<IQueryProcessor>());

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContextMock.Object);
            var handlerUnderTest = new HandlerUnderTest(httpContextAccessor);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new TestRequirement() },
                user: null,
                resource: null);

            // Act
            await handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerUnderTest.WasHandleRequirementCalled
                .Should().BeFalse();
        }

        [Fact]
        public async Task Does_Not_Handle_Requirements_When_User_Is_Not_Authenticated()
        {
            var projectId = Guid.NewGuid();
            var routeData = new RouteData();
            var routingFeature = new RoutingFeature { RouteData = routeData };
            routeData.Values.Add("projectId", projectId.ToString());

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock
                .Setup(x => x.Features[typeof(IRoutingFeature)])
                .Returns(routingFeature);

            var queryProcessorMock = new Mock<IQueryProcessor>();
            queryProcessorMock
                .Setup(x => x.FindById<Project>(projectId))
                .Returns(Mock.Of<Project>());

            httpContextMock
                .Setup(x => x.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(queryProcessorMock.Object);

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContextMock.Object);
            var handlerUnderTest = new HandlerUnderTest(httpContextAccessor);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new TestRequirement() },
                user: FakeClaimsPrincipalFactory.CreateAnonymousUser(),
                resource: null);

            // Act
            await handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerUnderTest.WasHandleRequirementCalled
                .Should().BeFalse();
        }

        [Fact]
        public async Task Handles_Requirements_When_Project_Is_Found_From_Route_And_User_Is_Authenticated()
        {
            var projectId = Guid.NewGuid();
            var routeData = new RouteData();
            var routingFeature = new RoutingFeature { RouteData = routeData };
            routeData.Values.Add("projectId", projectId.ToString());

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock
                .Setup(x => x.Features[typeof(IRoutingFeature)])
                .Returns(routingFeature);

            var queryProcessorMock = new Mock<IQueryProcessor>();
            queryProcessorMock
                .Setup(x => x.FindById<Project>(projectId))
                .Returns(Mock.Of<Project>());

            httpContextMock
                .Setup(x => x.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(queryProcessorMock.Object);

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContextMock.Object);
            var handlerUnderTest = new HandlerUnderTest(httpContextAccessor);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new TestRequirement() },
                user: FakeClaimsPrincipalFactory.CreateAuthenticatedUser(),
                resource: null);

            // Act
            await handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerUnderTest.WasHandleRequirementCalled
                .Should().BeTrue();
        }

        [Fact]
        public async Task Allows_unauthenticated_when_marked_as_such()
        {
            var projectId = Guid.NewGuid();
            var routeData = new RouteData();
            var routingFeature = new RoutingFeature { RouteData = routeData };
            routeData.Values.Add("projectId", projectId.ToString());

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock
                .Setup(x => x.Features[typeof(IRoutingFeature)])
                .Returns(routingFeature);

            var queryProcessorMock = new Mock<IQueryProcessor>();
            queryProcessorMock
                .Setup(x => x.FindById<Project>(projectId))
                .Returns(Mock.Of<Project>());

            httpContextMock
                .Setup(x => x.RequestServices.GetService(typeof(IQueryProcessor)))
                .Returns(queryProcessorMock.Object);

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == httpContextMock.Object);
            var handlerUnderTest = new HandlerUnderTest(httpContextAccessor);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new TestRequirement() },
                user: FakeClaimsPrincipalFactory.CreateAnonymousUser(),
                resource: null);

            handlerUnderTest.SetAllowUnauthenticated(true);

            // Act
            await handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerUnderTest.WasHandleRequirementCalled
                .Should().BeTrue();
        }

        private class HandlerUnderTest : ProjectAuthorizationHandler<TestRequirement>
        {
            public HandlerUnderTest(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            public bool WasHandleRequirementCalled { get; private set; }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestRequirement requirement, object resource = null)
            {
                WasHandleRequirementCalled = true;

                return Task.CompletedTask;
            }

            public void SetAllowUnauthenticated(bool value)
            {
                AllowUnauthenticated = value;
            }
        }

        private class TestRequirement : IAuthorizationRequirement
        {
        }
    }
}
