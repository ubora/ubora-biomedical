using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
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
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(
                userId: Guid.NewGuid(),
                fullName: nameof(ApplicationUser.FullNameClaimType) + Guid.NewGuid());

            return user;
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