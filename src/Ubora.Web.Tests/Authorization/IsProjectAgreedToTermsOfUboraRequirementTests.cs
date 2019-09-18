using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsProjectAgreedToTermsOfUboraRequirementTests
    {
        private readonly HandlerUnderTest _handlerUnderTest;

        public IsProjectAgreedToTermsOfUboraRequirementTests()
        {
            _handlerUnderTest = new HandlerUnderTest();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Succeeds_when_project_is_in_agreement_with_terms_of_UBORA(
            bool isAuthenticated)
        {
            ClaimsPrincipal user;
            if (isAuthenticated)
                user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(Guid.NewGuid());
            else
                user = FakeClaimsPrincipalFactory.CreateAnonymousUser();

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectAgreedToTermsOfUboraRequirement() },
                user: user,
                resource: null);

            var projectId = Guid.NewGuid();
            var project = new Project().Set(x => x.Id, projectId).Set(x => x.IsAgreedToTermsOfUbora, true);
            _handlerUnderTest.SetProject(project);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_NOT_succeed_when_project_is_NOT_in_agreement_with_terms_of_UBORA()
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: Guid.NewGuid());

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsProjectAgreedToTermsOfUboraRequirement() },
                user: user,
                resource: null);

            var projectId = Guid.NewGuid();
            var project = new Project().Set(x => x.Id, projectId).Set(x => x.IsAgreedToTermsOfUbora, false);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }

        private class HandlerUnderTest : IsProjectAgreedToTermsOfUboraRequirement.Handler
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
