using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Data;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsUboraAdminGenericRequirementHandler<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var isInAdminRole = context.User.IsInRole(ApplicationRole.Admin);
            if (isInAdminRole)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}