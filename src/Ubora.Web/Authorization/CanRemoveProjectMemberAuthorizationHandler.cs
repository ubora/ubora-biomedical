using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class CanRemoveProjectMemberAuthorizationHandler : AuthorizationHandler<CanRemoveProjectMemberRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanRemoveProjectMemberRequirement requirement)
        {
            var filterContext = (AuthorizationFilterContext)context.Resource;

            // Always succeed requirement when disabling filter is present. 
            // (It probably means that there is another less restrictive policy specified on derived action/controller.)
            var filters = filterContext.Filters;
            if (filters.Any(filter => filter is IDisablesProjectAuthorizationPolicyFilter))
            {
                context.Succeed(requirement);
                return;
            }

            var serviceProvider = filterContext.HttpContext.RequestServices;
            var routeData = filterContext.RouteData;
            var queryProcessor = (IQueryProcessor)serviceProvider.GetService(typeof(IQueryProcessor));

            var project = GetProject(routeData, queryProcessor);
            if (project == null)
            {
                context.Fail();
                return;
            }

            var user = context.User;
            if (!user.Identity.IsAuthenticated)
            {
                return;
            }

            var isMember = project.DoesSatisfy(new IsLeader(user.GetId()));
            if (isMember)
            {
                context.Succeed(requirement);
            }
        }

        private Project GetProject(RouteData routeData, IQueryProcessor queryProcessor)
        {
            Guid projectId = GetProjectId(routeData);
            var project = queryProcessor.FindById<Project>(projectId);

            return project;
        }

        private Guid GetProjectId(RouteData routeData)
        {
            var projectIdFromRoute = routeData.Values["projectId"] as string;
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            return projectId;
        }
    }
}
