using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class IsProjectMemberAuthorizationHandler : AuthorizationHandler<IsProjectMemberRequirement>
    {
        private readonly IQueryProcessor _queryProcessor;

        public IsProjectMemberAuthorizationHandler(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
        {
            var authorizationFilterContext = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext) context.Resource;

            // Always succeed requirement when disabling filter is present. 
            // (It probably means that there is another less restrictive policy specified on derived action/controller.)
            var filters = authorizationFilterContext.Filters;
            if (filters.Any(filter => filter is IDisablesProjectAuthorizationPolicyFilter))
            {
                context.Succeed(requirement);
                return;
            }

            // Get project's id from route.
            var projectIdFromRoute = authorizationFilterContext.RouteData.Values["projectId"] as string;
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            if (projectId == default(Guid))
            {
                context.Fail();
                return;
            }

            var user = context.User;
            if (!user.Identity.IsAuthenticated)
            {
                return;
            }

            var project = _queryProcessor.FindById<Project>(projectId);
            var isMember = project.DoesSatisfy(new HasMember(user.GetId()));
            if (isMember)
            {
                context.Succeed(requirement);
            }
        }
    }
}