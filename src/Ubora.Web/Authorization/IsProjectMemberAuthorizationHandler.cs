using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Services;
using Autofac;

namespace Ubora.Web.Authorization
{
    public class IsProjectMemberAuthorizationHandler : AuthorizationHandler<IsProjectMemberRequirement>
    {
        private readonly IIsMemberPartOfProject _isMemberPartOfProject;

        public IsProjectMemberAuthorizationHandler(IIsMemberPartOfProject isMemberPartOfProject)
        {
            _isMemberPartOfProject = isMemberPartOfProject;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
        {
            var authorizationFilterContext = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)context.Resource;

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

            // TODO: Make pretty
            var isMember = _isMemberPartOfProject.Satisfy(projectId, user.GetId());
            if (isMember)
            {
                context.Succeed(requirement);
            }
        }
    }
}