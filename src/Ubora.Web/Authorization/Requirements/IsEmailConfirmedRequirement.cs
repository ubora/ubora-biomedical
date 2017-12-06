using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsEmailConfirmedRequirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<IsEmailConfirmedRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEmailConfirmedRequirement requirement)
            {
                var isEmailConfirmed = context.User.IsEmailConfirmed();
                if (isEmailConfirmed)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
