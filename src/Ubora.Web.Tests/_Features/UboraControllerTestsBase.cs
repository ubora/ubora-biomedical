using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Ubora.Web._Features;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;

namespace Ubora.Web.Tests._Features
{
    public class UboraControllerTestsBase
    {
        protected IPrincipal User { get; }
        protected Guid UserId { get; }
        protected ModelStateDictionary ModelState { get; private set; }

        public UboraControllerTestsBase()
        {
            UserId = Guid.NewGuid();
            User = CreateUser(UserId);
        }

        protected virtual ClaimsPrincipal CreateUser(Guid userId)
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(
                userId: userId,
                fullName: nameof(ApplicationUser.FullNameClaimType) + Guid.NewGuid());

            return user;
        }

        protected void SetUserContext(UboraController controller)
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
        }
    }
}
