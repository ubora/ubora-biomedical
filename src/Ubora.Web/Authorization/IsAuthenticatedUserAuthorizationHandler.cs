using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ubora.Web.Authorization
{
    public class IsAuthenticatedUserAuthorizationHandler : AuthorizationHandler<IsAuthenticatedUserRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthenticatedUserRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
        }
    }
}