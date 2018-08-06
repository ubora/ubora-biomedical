using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsProjectMemberRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectMemberRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
            {
                var isMentor = Project.DoesSatisfy(new HasMember<UserProfile>(context.User.GetId()));
                if (isMentor)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}