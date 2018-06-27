using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Ubora.Domain.Projects.Members;

namespace Ubora.Web.Authorization.Requirements
{
    public class HasProjectMemberRequirement<T> : IAuthorizationRequirement where T : ProjectMember
    {
        public class Handler : AuthorizationHandler<HasProjectMemberRequirement<T>, ProjectMember>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasProjectMemberRequirement<T> requirement, ProjectMember projectMember)
            {

                if (projectMember is T)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
