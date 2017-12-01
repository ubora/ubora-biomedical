using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Ubora.Web.Authorization.Requirements
{
    /// <summary> 'Special' requirement meant only for use on <see cref="_Features.Projects.ProjectController"/>. </summary>
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

                // Authorize differently based on HTTP method.
                var httpMethod = new HttpMethod(filterContext.HttpContext.Request.Method);
                bool isAuthorized;

                if (httpMethod == HttpMethod.Get)
                {
                    isAuthorized = await authorizationService.IsAuthorizedAsync(context.User, Policies.CanViewProjectNonPublicContent);
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    isAuthorized = await authorizationService.IsAuthorizedAsync(context.User, Policies.CanWorkOnProjectContent);
                }
                // Other HTTP methods not yet accounted for.
                else
                {
                    isAuthorized = false;
                }

                if (isAuthorized)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}