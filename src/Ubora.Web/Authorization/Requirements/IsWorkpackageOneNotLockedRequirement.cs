using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsWorkpackageOneNotLockedRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsWorkpackageOneNotLockedRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsWorkpackageOneNotLockedRequirement requirement)
            {
                var workpackageOne = QueryProcessor.FindById<WorkpackageOne>(Project.Id);

                if (!workpackageOne.IsLocked)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
