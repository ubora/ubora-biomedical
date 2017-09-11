using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
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

namespace Ubora.Web.Tests._Features
{
    public abstract class UboraControllerTestsBase
    {
        protected ClaimsPrincipal User { get; }
        protected Guid UserId { get; }

        public Mock<IQueryProcessor> QueryProcessorMock { get; private set; } = new Mock<IQueryProcessor>();
        public Mock<ICommandProcessor> CommandProcessorMock { get; private set; } = new Mock<ICommandProcessor>(MockBehavior.Strict);
        public Mock<IMapper> AutoMapperMock { get; private set; } = new Mock<IMapper>();
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; private set; } = new Mock<IAuthorizationService>();

        protected UboraControllerTestsBase()
        {
            UserId = Guid.NewGuid();
            User = CreateUser(UserId);
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
            serviceProviderMock.Setup(x => x.GetService(typeof(ICommandProcessor))).Returns(CommandProcessorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IQueryProcessor))).Returns(QueryProcessorMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IMapper))).Returns(AutoMapperMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IAuthorizationService))).Returns(AuthorizationServiceMock.Object);

            // Stub ASP.NET MVC services
            serviceProviderMock.Setup(x => x.GetService(typeof(IUrlHelperFactory))).Returns(Mock.Of<IUrlHelperFactory>());
            serviceProviderMock
                .Setup(x => x.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(Mock.Of<ITempDataDictionaryFactory>(f => f.GetTempData(controller.ControllerContext.HttpContext) == new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>())));
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

        public void AssertHasAuthorizeAttributes(Type controller, List<RolesAndPoliciesAuthorization> rolesAndPoliciesAuthorizations)
        {
            var authorizedControllerMethods = GetAuthorizedControllerMethods(controller);

            var errorMessages = new List<string>();

            foreach (var authorizedControllerMethod in authorizedControllerMethods)
            {
                HasMethodName(controller, rolesAndPoliciesAuthorizations);
                HasAttributesbeenDuplicated(controller, authorizedControllerMethod, errorMessages);

                var hasMethodbeenTested = HasMethodbeenTested(authorizedControllerMethod, rolesAndPoliciesAuthorizations, errorMessages);
                if (hasMethodbeenTested)
                {
                    var rolesAndPoliciesAuthorization = rolesAndPoliciesAuthorizations.SingleOrDefault(a => a.MethodName == authorizedControllerMethod.Name);
                    HasRolesAndPoliciesbeenTested(authorizedControllerMethod, rolesAndPoliciesAuthorization, errorMessages);
                }
            }

            if (errorMessages.Count >= 1)
            {
                Assert.False(true, $"{string.Join(string.Empty, errorMessages)}");
            }
        }

        private static IEnumerable<MethodInfo> GetAuthorizedControllerMethods(Type controller)
        {
            return controller.GetMethods().Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && method.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());
        }

        private static void HasMethodName(Type controller, List<RolesAndPoliciesAuthorization> authorizations)
        {
            foreach (var authorization in authorizations)
            {
                if (controller.GetMethods().All(m => m.Name != authorization.MethodName))
                {
                    Assert.False(true, $"HasAttribute controller.method:  '{controller.Name}.{authorization.MethodName}' does not exist  - copy/paste ?");
                }
            }
        }

        private static void HasAttributesbeenDuplicated(Type controller, MemberInfo methodInfo, List<string> errors)
        {
            var controllerAttributes = Attribute.GetCustomAttribute(controller, typeof(AuthorizeAttribute));

            foreach (var methodAttributes in methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true))
            {
                if (Equals(methodAttributes, controllerAttributes))
                {
                    errors.Add($"Duplicated action {methodInfo.Name} and controller {controller.Name} attributes! \n");
                }
            }
        }

        private static bool HasMethodbeenTested(MethodInfo authorizedControllerMethod, IEnumerable<RolesAndPoliciesAuthorization> rolesAndPoliciesAuthorizations, List<string> errors)
        {
            var hasMethodbeenTested = rolesAndPoliciesAuthorizations.Select(x => x.MethodName).Contains(authorizedControllerMethod.Name);

            if (!hasMethodbeenTested)
            {
                errors.Add($"{authorizedControllerMethod.Name} was not tested \n");
                return false;
            }

            return true;
        }

        private static void HasRolesAndPoliciesbeenTested(MethodInfo authorizedControllerMethod, RolesAndPoliciesAuthorization rolesAndPoliciesAuthorization, List<string> errors)
        {
            foreach (var attribute in authorizedControllerMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true))
            {
                if (((AuthorizeAttribute)attribute).Policy != null)
                {
                    if (rolesAndPoliciesAuthorization.Policies == null)
                    {
                        errors.Add($"{authorizedControllerMethod.Name} {((AuthorizeAttribute)attribute).Policy} policy was not tested \n");
                    }
                    else
                    {
                        var hasPoliciesbeenTested =
                            rolesAndPoliciesAuthorization.Policies.Contains(((AuthorizeAttribute)attribute).Policy);
                        if (!hasPoliciesbeenTested)
                        {

                            errors.Add($"{authorizedControllerMethod.Name} {((AuthorizeAttribute)attribute).Policy} policy was not tested \n");
                        }
                    }
                }
                else
                {
                    if (rolesAndPoliciesAuthorization.Policies != null)
                    {
                        errors.Add($"{authorizedControllerMethod.Name}.{attribute}.{((AuthorizeAttribute)attribute).Policy} policy haven't in controller \n");
                    }
                }

                if (((AuthorizeAttribute)attribute).Roles != null)
                {
                    if (rolesAndPoliciesAuthorization.Roles == null)
                    {
                        errors.Add($"{authorizedControllerMethod.Name} {((AuthorizeAttribute)attribute).Roles} roles was not tested \n");
                    }
                    else
                    {
                        var hasRolesbeenTested = rolesAndPoliciesAuthorization.Roles.Contains(((AuthorizeAttribute)attribute).Roles);
                        if (!hasRolesbeenTested)
                        {
                            errors.Add($"{authorizedControllerMethod.Name} {((AuthorizeAttribute)attribute).Roles} roles was not tested \n");
                        }
                    }
                }
                else
                {
                    if (rolesAndPoliciesAuthorization.Roles != null)
                    {
                        errors.Add($"{authorizedControllerMethod.Name}.{attribute}.{((AuthorizeAttribute)attribute).Roles} roles haven't in controller \n");
                    }
                }
            }
        }

        public class RolesAndPoliciesAuthorization
        {
            public string MethodName { get; set; }
            public IEnumerable<string> Policies { get; set; }
            public IEnumerable<string> Roles { get; set; }
        }
    }
}
