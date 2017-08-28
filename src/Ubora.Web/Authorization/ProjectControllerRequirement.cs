using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Ubora.Web.Authorization
{
    /// <see cref="_Features.Projects.ProjectController"/>
    public class ProjectControllerRequirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<ProjectControllerRequirement>
        {
            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectControllerRequirement requirement)
            {
                var filterContext = (AuthorizationFilterContext)context.Resource; // Assumes that resource is passed by controller action.

                // Always succeed requirement when disabling filter is present. 
                // (It probably means that there is another less restrictive policy specified on derived action/controller.)
                var filters = filterContext.Filters;
                if (filters.Any(filter => filter is IDisablesProjectControllerAuthorizationFilter))
                {
                    context.Succeed(requirement);
                    return;
                }

                // Use service location for circular dependency workaround.
                var serviceProvider = filterContext.HttpContext.RequestServices;
                var authorizationService = serviceProvider.GetService<IAuthorizationService>();

                var authorizationResult = await authorizationService.AuthorizeAsync(context.User,
                    resource: null,
                    requirements: new IAuthorizationRequirement[]
                    {
                        new DenyAnonymousAuthorizationRequirement(),
                        new IsProjectMemberRequirement()
                    });

                if (authorizationResult.Succeeded)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}