using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsWorkpackageOneNotLockedRequirementHandlerTests
    {
        private readonly HandlerUnderTest _handlerUnderTest;

        public IsWorkpackageOneNotLockedRequirementHandlerTests()
        {
            _handlerUnderTest = new HandlerUnderTest();
        }

        [Fact]
        public async Task Succeeds_When_Workpackage_One_Is_Not_Locked()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsWorkpackageOneNotLockedRequirement() },
                user: user,
                resource: null);

            var projectId = Guid.NewGuid();
            var project = new Project().Set(x => x.Id, projectId);
            _handlerUnderTest.SetProject(project);

            var workpackageOneMock = new Mock<WorkpackageOne>();
            var queryProcessor = Mock.Of<IQueryProcessor>(x => x.FindById<WorkpackageOne>(projectId) == workpackageOneMock.Object);
            _handlerUnderTest.SetQueryProcessor(queryProcessor);

            workpackageOneMock
                .Setup(x => x.DoesSatisfy(new IsWorkpackageOneLocked()))
                .Returns(false);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_Not_Succeed_When_Workpackage_One_Is_Locked()
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: Guid.NewGuid());

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsWorkpackageOneNotLockedRequirement() },
                user: user,
                resource: null);

            var projectId = Guid.NewGuid();
            var project = new Project().Set(x => x.Id, projectId);
            _handlerUnderTest.SetProject(project);

            var workpackageOneMock = new Mock<WorkpackageOne>();
            var queryProcessor = Mock.Of<IQueryProcessor>(x => x.FindById<WorkpackageOne>(projectId) == workpackageOneMock.Object);
            _handlerUnderTest.SetQueryProcessor(queryProcessor);

            workpackageOneMock
                .Setup(x => x.DoesSatisfy(new IsWorkpackageOneLocked()))
                .Returns(true);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }

        private class HandlerUnderTest : IsWorkpackageOneNotLockedRequirement.Handler
        {
            private Project _project = Mock.Of<Project>();
            private IQueryProcessor _queryProcessor = Mock.Of<IQueryProcessor>();

            public HandlerUnderTest()
                : base(Mock.Of<IHttpContextAccessor>(x => x.HttpContext == Mock.Of<HttpContext>()))
            {
            }

            protected override IQueryProcessor QueryProcessor => _queryProcessor;

            internal void SetProject(Project project)
            {
                _project = project;
            }

            internal void SetQueryProcessor(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            protected override Project GetProject()
            {
                return _project;
            }
        }
    }
}
