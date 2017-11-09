using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
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
