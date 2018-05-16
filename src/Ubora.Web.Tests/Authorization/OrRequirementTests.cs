using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Web.Authorization.Requirements;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class OrRequirementTests
    {
        private readonly OrRequirement.Handler _handler;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;

        public OrRequirementTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _httpContextAccessorMock.Setup(a => a.HttpContext.RequestServices.GetService(typeof(IAuthorizationService))).Returns(_authorizationServiceMock.Object);
            _handler = new OrRequirement.Handler(_httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_Is_Succeed_When_Both_Requirements_Is_Succeed()
        {
            var handlerContext = new AuthorizationHandlerContext(
                    requirements: new[]
                        { new OrRequirement(new IsProjectLeaderRequirement(), new IsProjectMentorRequirement()) },
                    user: null,
                    resource: null);

            _authorizationServiceMock
                .SetupSequence(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthorizationHandlerContext>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success())
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Is_Succeed_When_Requirement_Is_Succeed()
        {
            var handlerContext = new AuthorizationHandlerContext(
                    requirements: new[]
                        { new OrRequirement(new IsProjectLeaderRequirement()) },
                    user: null,
                    resource: null);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthorizationHandlerContext>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Is_Succeed_When_Given_Requirement_Is_Succeed_And_The_Another_Is_Failed()
        {
            var handlerContext = new AuthorizationHandlerContext(
                    requirements: new[]
                        { new OrRequirement(new IsProjectLeaderRequirement(), new IsProjectMentorRequirement()) },
                    user: null,
                    resource: null);

            _authorizationServiceMock
                .SetupSequence(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthorizationHandlerContext>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success())
                .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Is_Failed_When_Both_Requirements_Is_Failed()
        {
            var handlerContext = new AuthorizationHandlerContext(
                    requirements: new[]
                        { new OrRequirement(new IsProjectLeaderRequirement(), new IsProjectMentorRequirement()) },
                    user: null,
                    resource: null);

            _authorizationServiceMock
                .SetupSequence(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthorizationHandlerContext>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed())
                .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Is_Failed_When_Is_Empty_Requirement()
        {
            var handlerContext = new AuthorizationHandlerContext(
                    requirements: new[]
                        { new OrRequirement() },
                    user: null,
                    resource: null);

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Is_Failed_When_Context_Has_Failed()
        {
            var handlerContext = new AuthorizationHandlerContext(requirements: new[] { new OrRequirement(new IsProjectLeaderRequirement()) }, user: null, resource: null);
            handlerContext.Fail();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthorizationHandlerContext>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success());

            // Act
            await _handler.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded.Should().BeFalse();
        }
    }
}
