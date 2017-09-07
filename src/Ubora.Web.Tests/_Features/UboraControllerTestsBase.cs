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
using FluentAssertions;
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

            var authorizedControllerMethods = controller.Assembly.Types()
                .Where(controller.IsAssignableFrom)
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) &&
                                 method.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());

            foreach (var authorizedControllerMethod in authorizedControllerMethods)
            {
                var hasMethodbeenTested = rolesAndPoliciesAuthorizations.Select(x => x.MethodName).Contains(authorizedControllerMethod.Name);
                if (!hasMethodbeenTested)
                {
                    Assert.False(true, $"{controller.Name}.{authorizedControllerMethod} was not tested");
                }

                var authorizedControllerMethodPolicies = authorizedControllerMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Where(a => ((AuthorizeAttribute)a).Policy != null).Select(x => ((AuthorizeAttribute)x).Policy).ToList();

                if (authorizedControllerMethodPolicies.Count >= 1)
                {

                    foreach (var rolesAndPoliciesAuthorization in rolesAndPoliciesAuthorizations)
                    {
                        AssertHasAttribute(controller, rolesAndPoliciesAuthorization.MethodName, rolesAndPoliciesAuthorization.Policies);

                        if (rolesAndPoliciesAuthorization.MethodName == authorizedControllerMethod.Name)
                        {
                            foreach (var policy in rolesAndPoliciesAuthorization.Policies)
                            {
                                var hasPoliciesbeenTested = authorizedControllerMethodPolicies.Contains(policy);

                                if (!hasPoliciesbeenTested)
                                {
                                    Assert.False(true, $"{controller.Name}.{authorizedControllerMethod}.{policy} was not tested");
                                }
                            }
                        }
                    }
                }
            }
        }

        public class RolesAndPoliciesAuthorization
        {
            public string MethodName { get; set; }
            public List<string> Policies { get; set; }
            public List<string> Roles { get; set; }
        }

        private void AssertHasAttribute(Type controller, string methodName, List<string> attributePolicies)
        {
            var methodInfos = GetMethodInfos(controller, methodName);

            var customAttributeController = Attribute.GetCustomAttribute(controller, typeof(AuthorizeAttribute));

            foreach (var customAttributes in methodInfos.Select(i => i.GetCustomAttributes(typeof(AuthorizeAttribute), true)))
            {
                if (customAttributes.Contains(customAttributeController))
                {
                    Assert.False(true, $"Duplicated action and controller attributes!");
                }

                var attributes = customAttributes.ToList();
                attributes.Add(customAttributeController);

                if (attributePolicies.Any())
                {
                    var policies = attributes.Select(a => ((AuthorizeAttribute)a).Policy);
                    Assert.True(policies.Intersect(attributePolicies).Any());
                }
            }
        }

        private static IEnumerable<MethodInfo> GetMethodInfos(Type controller, string methodName)
        {
            if (controller.GetMethods().All(m => m.Name != methodName))
            {
                Assert.False(true, $"HasAttribute controller.method:  '{controller.Name}.{methodName}' does not exist  - copy/paste ? :)");
            }

            return controller.GetMethods().Where(m => m.Name == methodName);
        }
    }
}
