using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsProjectLeaderOrMentorOrAdminRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectLeaderOrMentorOrAdminRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectLeaderOrMentorOrAdminRequirement requirement)
            {
                var userId = context.User.GetId();
                var isLeaderOrMentor = Project.DoesSatisfy(new HasLeader(userId) || new HasMember<ProjectMentor>(userId));
                var isInAdminRole = context.User.IsInRole(ApplicationRole.Admin);
                if (isLeaderOrMentor || isInAdminRole)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
