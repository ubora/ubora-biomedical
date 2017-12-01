﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
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
        public async Task Handler_Allows_Anonymous_Pass_When_Disabling_Filter_Is_Present_On_Controller_Action()
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
        public async Task Handler_Authorizes_For_Get_HTTP_Requests_Whether_User_Can_View_Non_Public_Content_Of_Given_Project(
            bool isAuthorized)
        {
            var httpContextMock = new Mock<HttpContext>();

            httpContextMock
                .Setup(x => x.Request.Method)
                .Returns(HttpMethod.Get.ToString);

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

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(currentUser, null, Policies.CanViewProjectNonPublicContent))
                .ReturnsAsync(isAuthorized ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().Be(isAuthorized);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handler_Authorizes_For_Post_HTTP_Requests_Whether_User_Can_Work_On_Project_Content(
            bool isAuthorized)
        {
            var httpContextMock = new Mock<HttpContext>();

            httpContextMock
                .Setup(x => x.Request.Method)
                .Returns(HttpMethod.Post.ToString);

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

            authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(currentUser, null, Policies.CanWorkOnProjectContent))
                .ReturnsAsync(isAuthorized ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().Be(isAuthorized);
        }
    }
}