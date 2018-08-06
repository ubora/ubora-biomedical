using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Specifications;

namespace Ubora.Web.Authorization.Requirements
{
    public class HasProjectMemberOfTypeRequirement<T> : IAuthorizationRequirement where T : UserProfile
    {
        public class Handler : ProjectAuthorizationHandler<HasProjectMemberOfTypeRequirement<T>, UserProfile>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }
            
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasProjectMemberOfTypeRequirement<T> requirement, UserProfile userProfile)
            {
                if (Project.GetMembers(new HasUserIdSpec(userProfile.UserId)).Any(m => m is T))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
