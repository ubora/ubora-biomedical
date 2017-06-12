using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Ubora.Web._Features;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;

namespace Ubora.Web.Tests._Features
{
    public class UboraControllerTestsBase
    {
        protected IPrincipal User { get; }

        public UboraControllerTestsBase()
        {
            User = CreateUser();
        }

        protected virtual ClaimsPrincipal CreateUser()
        {
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(
                userId: Guid.NewGuid(),
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
        }
    }
}
