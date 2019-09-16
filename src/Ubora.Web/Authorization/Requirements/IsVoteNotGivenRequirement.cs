using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsVoteNotGivenRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsVoteNotGivenRequirement, Candidate>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                IsVoteNotGivenRequirement requirement,
                Candidate candidate)
            {
                var hasUserVoted = candidate.Votes?.Any(x => x.UserId == context.User.GetId()) ?? false;
                if (!hasUserVoted)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
