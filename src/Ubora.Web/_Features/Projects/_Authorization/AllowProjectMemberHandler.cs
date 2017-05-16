using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects._Authorization
{
    public class AllowProjectMemberHandler : AuthorizationHandler<IsProjectMemberRequirement>
    {
        private readonly IQuerySession _querySession;

        public AllowProjectMemberHandler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
        {
            var authorizationFilterContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            var httpContext = authorizationFilterContext.HttpContext;

            var projectIdFromRoute = (string)httpContext.GetRouteValue("projectId");
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            if (projectId != Guid.Empty)
            {
                var project = _querySession.Load<Project>(projectId);
                var user = context.User;

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
