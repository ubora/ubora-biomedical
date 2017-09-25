using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class IsProjectMemberRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectMemberRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor)
                : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectMemberRequirement requirement)
            {
                var isMember = Project.DoesSatisfy(new HasMember(context.User.GetId()));
                if (isMember)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}