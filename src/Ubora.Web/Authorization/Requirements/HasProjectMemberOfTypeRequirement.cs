using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Specifications;

namespace Ubora.Web.Authorization.Requirements
{
    public class HasProjectMemberOfTypeRequirement<T> : IAuthorizationRequirement where T : ProjectMember
    {
        public class Handler : ProjectAuthorizationHandler<HasProjectMemberOfTypeRequirement<T>, ProjectMember>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }
            
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasProjectMemberOfTypeRequirement<T> requirement, ProjectMember projectMember)
            {
                if (Project.GetMembers(new HasUserIdSpec(projectMember.UserId)).Any(m => m is T))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
