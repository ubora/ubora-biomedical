using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Tests.Helper;

namespace Ubora.Web.Tests._Features
{
    public abstract class UboraControllerTestsBase
    {
        protected ClaimsPrincipal User { get; }
        protected Guid UserId { get; }
        protected ModelStateDictionary ModelState { get; private set; }

        public Mock<IQueryProcessor> QueryProcessorMock { get; private set; } = new Mock<IQueryProcessor>();
        public Mock<ICommandProcessor> CommandProcessorMock { get; private set; } = new Mock<ICommandProcessor>();
        public Mock<IMapper> AutoMapperMock { get; private set; } = new Mock<IMapper>();
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; private set; } = new Mock<IAuthorizationService>();

        protected UboraControllerTestsBase()
        {
            UserId = Guid.NewGuid();
            User = CreateUser(UserId);
        }

        protected T SetMocks<T>(T controller) where T : UboraController
        {
            controller
                .Set(x => x.QueryProcessor, this.QueryProcessorMock.Object)
                .Set(x => x.CommandProcessor, this.CommandProcessorMock.Object)
                .Set(x => x.AutoMapper, this.AutoMapperMock.Object)
                .Set(x => x.AuthorizationService, this.AuthorizationServiceMock.Object);

            return controller;
        }

        protected T SetUserContext<T>(T controller) where T : UboraController
        {
            controller.ControllerContext = new ControllerContext(new ActionContext
            {
                RouteData = new RouteData(),
                HttpContext = new DefaultHttpContext
                {
                    User = (ClaimsPrincipal)User
                },
                ActionDescriptor = new ControllerActionDescriptor()
            });
            ModelState = controller.ModelState;

            return controller;
        }

        protected virtual ClaimsPrincipal CreateUser(Guid userId)
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(
                userId: userId,
                fullName: nameof(ApplicationUser.FullNameClaimType) + Guid.NewGuid());

            return user;
        }

        protected void AssertModelStateContainsError(ViewResult viewResult, params string[] result)
        {
            foreach (var error in viewResult.ViewData.ModelState.Root.Errors)
            {
                Assert.Contains(error.ErrorMessage, result);
            }
        }
    }
}
