using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsProjectLeaderOrManagementGroupRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectLeaderOrManagementGroupRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectLeaderOrManagementGroupRequirement requirement)
            {
                var userId = context.User.GetId();
                var isLeader = Project.DoesSatisfy(new HasLeader(userId));
                var isInManagementGroupRole = context.User.IsInRole(ApplicationRole.ManagementGroup);
                if (isLeader || isInManagementGroupRole)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
