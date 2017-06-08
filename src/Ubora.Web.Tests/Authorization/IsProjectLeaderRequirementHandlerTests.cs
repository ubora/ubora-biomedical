using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsProjectLeaderRequirementHandlerTests
    {
        private readonly HandlerUnderTest _handlerUnderTest;

        public IsProjectLeaderRequirementHandlerTests()
        {
            _handlerUnderTest = new HandlerUnderTest();
        }

        [Fact]
        public async Task Succeeds_When_User_Is_Project_Leader()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectLeaderRequirement() },
                user: user,
                resource: null);

            var projectMock = new Mock<Project>();
            _handlerUnderTest.SetProject(projectMock.Object);

            projectMock
                .Setup(x => x.DoesSatisfy(new HasLeader(userId)))
                .Returns(true);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_Not_Succeed_When_User_Is_Not_Project_Leader()
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: Guid.NewGuid());

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectLeaderRequirement() },
                user: user,
                resource: null);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }

        private class HandlerUnderTest : IsProjectLeaderRequirement.Handler
        {
            private Project _project = Mock.Of<Project>();

            public HandlerUnderTest() 
                : base(Mock.Of<IHttpContextAccessor>(x => x.HttpContext == Mock.Of<HttpContext>()), Mock.Of<IQueryProcessor>())
            {
            }

            internal void SetProject(Project project)
            {
                _project = project;
            }

            protected override Project GetProject()
            {
                return _project;
            }
        }
    }
}
