using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsProjectLeaderRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectLeaderRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectLeaderRequirement requirement, object resource = null)
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
