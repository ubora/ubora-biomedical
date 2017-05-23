using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ubora.Web.Authorization
{
    public class IsAuthenticatedUserAuthorizationHandler : AuthorizationHandler<IsAuthenticatedUserRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthenticatedUserRequirement requirement)
        {
            var userIdentity = context?.User.Identity;
            if (userIdentity == null)
            {
                return;
            }

            if (userIdentity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
        }
    }
}