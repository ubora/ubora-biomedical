using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Ubora.Web.Data;
using Ubora.Web._Features.Projects;

namespace Ubora.Web.Tests._Features.Projects
{
    public abstract class ProjectControllerTestsBase
    {
        protected Guid ProjectId { get; }
        protected IPrincipal User { get; }

        protected ProjectControllerTestsBase()
        {
            ProjectId = Guid.NewGuid();
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

        protected void SetProjectAndUserContext(ProjectController controller)
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

            controller.RouteData.Values
                .Add("projectId", ProjectId.ToString());
        }
    }
}