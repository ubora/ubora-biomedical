using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects._Authorization
{
    public class IsCurrentProjectMember : IAuthorizationRequirement
    {
    }

    public class ProjectAuthorizationHandler : AuthorizationHandler<IsCurrentProjectMember>
    {
        private readonly IQuerySession _querySession;

        public ProjectAuthorizationHandler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCurrentProjectMember requirement)
        {
            var authorizationFilterContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            var httpContext = authorizationFilterContext.HttpContext;

            var user = context.User;

            var projectIdFromRoute = (string) httpContext.GetRouteValue("projectId");
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            if (projectId != Guid.Empty)
            {
                var project = _querySession.Load<Project>(projectId);

                if (user.Identity.IsAuthenticated)
                {
                    var isMember = project.DoesSatisfy(new HasMember(user.GetId()));
                    if (isMember)
                    {
                        context.Succeed(requirement);

                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
