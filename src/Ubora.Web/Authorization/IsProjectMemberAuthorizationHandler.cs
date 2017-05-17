using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class IsProjectMemberAuthorizationHandler : AuthorizationHandler<IsProjectMemberRequirement>
    {
        private readonly IQuerySession _querySession;

        public IsProjectMemberAuthorizationHandler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
        {
            var authorizationFilterContext = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext) context.Resource;

            var filters = authorizationFilterContext.ActionDescriptor.FilterDescriptors;
            if (filters.Any(x => x.Filter is IOverrideProjectPolicy))
            {
                context.Succeed(requirement);
            }

            var httpContext = authorizationFilterContext.HttpContext;

            var projectIdFromRoute = (string)httpContext.GetRouteValue("projectId");
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            if (projectId == Guid.Empty)
            {
                context.Fail();
                return;
            }

            var project = _querySession.Load<Project>(projectId);
            var user = context.User;

            if (!user.Identity.IsAuthenticated)
            {
                return;
            }

            var isMember = project.DoesSatisfy(new HasMember(user.GetId()));
            if (isMember)
            {
                context.Succeed(requirement);
            }
        }
    }
}