using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Tests.Helper;

namespace Ubora.Web.Tests._Features
{
    public abstract class UboraControllerTestsBase
    {
        protected ClaimsPrincipal User { get; }
        protected Guid UserId { get; }
        public Mock<IQueryProcessor> QueryProcessorMock { get; private set; } = new Mock<IQueryProcessor>();
        public Mock<IMapper> AutoMapperMock { get; private set; } = new Mock<IMapper>();

        public Mock<IAuthorizationService> AuthorizationServiceMock { get; private set; } =
            new Mock<IAuthorizationService>();

        public Mock<ICommandProcessor> CommandProcessorMock { get; private set; } = new Mock<ICommandProcessor>(MockBehavior.Strict);
        protected void AssertZeroCommandsExecuted() => CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);

        protected UboraControllerTestsBase()
        {
            UserId = Guid.NewGuid();
            User = CreateUser(UserId);
            AuthorizationServiceMock.SetReturnsDefault(Task.FromResult(AuthorizationResult.Failed()));
        }

        protected virtual void SetUpForTest(UboraController controller)
        {
            if (controller.ControllerContext.HttpContext == null)
            {
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
            }
            var serviceProviderMock = new Mock<IServiceProvider>();
            controller.ControllerContext.HttpContext.RequestServices = serviceProviderMock.Object;

            // Set user (ClaimsPrincipal)
            controller.ControllerContext.HttpContext.User = User;

            // Mock common Ubora services
            serviceProviderMock.Setup(x => x.GetService(typeof(ICommandProcessor)))
                .Returns(CommandProcessorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IQueryProcessor))).Returns(QueryProcessorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IMapper))).Returns(AutoMapperMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IAuthorizationService)))
                .Returns(AuthorizationServiceMock.Object);

            // Stub ASP.NET MVC services
            serviceProviderMock.Setup(x => x.GetService(typeof(IUrlHelperFactory)))
                .Returns(Mock.Of<IUrlHelperFactory>());
            serviceProviderMock
                .Setup(x => x.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(Mock.Of<ITempDataDictionaryFactory>(f =>
                    f.GetTempData(controller.ControllerContext.HttpContext) ==
                    new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>())));
        }

        [Fact]
        public virtual void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
            };

            AssertHasAuthorizeAttributes(typeof(UboraController), methodPolicies);
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

        protected void AssertHasAuthorizeAttributes(Type controller, List<AuthorizationTestHelper.RolesAndPoliciesAuthorization> rolesAndPoliciesAuthorizations)
        {
            AuthorizationTestHelper.AssertHasAuthorizeAttributes(controller, rolesAndPoliciesAuthorizations);
        }
    }
}
