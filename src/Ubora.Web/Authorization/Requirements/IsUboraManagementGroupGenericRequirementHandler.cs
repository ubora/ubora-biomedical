using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Ubora.Web.Data;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsUboraManagementGroupGenericRequirementHandler<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var isInManagementGroupRole = context.User.IsInRole(ApplicationRole.ManagementGroup);

            if (isInManagementGroupRole)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
