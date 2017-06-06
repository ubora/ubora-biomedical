using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Ubora.Web.Data;
using Ubora.Web._Features.Users.Manage;
using Xunit;

namespace Ubora.Web.Tests._Features.Users
{
    public abstract class UserControllerTestsBase
    {
        protected IPrincipal User { get; }

        protected UserControllerTestsBase()
        {
            User = CreateUser();
        }

        protected virtual ClaimsPrincipal CreateUser()
        {
            var claims = new[]
            {
                new Claim(ApplicationUser.FullNameClaimType, value: nameof(ApplicationUser.FullNameClaimType) + Guid.NewGuid()),
                new Claim(ClaimTypes.NameIdentifier, value: Guid.NewGuid().ToString()),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        protected void SetManageAndUserContext(ManageController controller)
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

        protected void AssertModelStateContainsError(ViewResult viewResult, params string[] result)
        {
            foreach (var error in viewResult.ViewData.ModelState.Root.Errors)
            {
                Assert.Contains(error.ErrorMessage, result);
            }
        }
    }
}
