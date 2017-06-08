using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization
{
    public class IsProjectMemberRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsProjectMemberRequirement>
        {
            public Handler(IHttpContextAccessor httpContextAccessor, IQueryProcessor queryProcessor)
                : base(httpContextAccessor, queryProcessor)
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