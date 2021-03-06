﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsVoteNotGivenRequirementHandlerTests
    {
        private readonly HandlerUnderTest _handlerUnderTest;

        public IsVoteNotGivenRequirementHandlerTests()
        {
            _handlerUnderTest = new HandlerUnderTest();
            _handlerUnderTest.SetProject(new Project());
        }

        [Fact]
        public async Task Succeeds_When_User_Has_Not_Voted_For_Candidate()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: userId);

            var vote1 = new Vote(Guid.NewGuid(), 1, 2, 3, 4);
            var vote2 = new Vote(Guid.NewGuid(), 1, 2, 3, 4);
            var candidate = new Mock<Candidate>();
            candidate.Setup(x => x.Votes)
                .Returns(new[] { vote1, vote2 });

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsVoteNotGivenRequirement() },
                user: user,
                resource: candidate.Object);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_Not_Succeed_When_User_Has_Not_Voted_For_Candidate()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: userId);

            var vote1 = new Vote(userId, 1, 2, 3, 4);
            var vote2 = new Vote(Guid.NewGuid(), 1, 2, 3, 4);
            var candidate = new Mock<Candidate>();
            candidate.Setup(x => x.Votes)
                .Returns(new[] { vote1, vote2 });

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsVoteNotGivenRequirement() },
                user: user,
                resource: candidate.Object);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }

        private class HandlerUnderTest : IsVoteNotGivenRequirement.Handler
        {
            private Project _project = Mock.Of<Project>();

            public HandlerUnderTest()
                : base(Mock.Of<IHttpContextAccessor>(x => x.HttpContext == Mock.Of<HttpContext>()))
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
