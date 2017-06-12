using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class IsProjectLeaderRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectLeaderRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectLeaderRequirement requirement)
            {
                var isLeader = Project.DoesSatisfy(new HasLeader(context.User.GetId()));
                if (isLeader)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
