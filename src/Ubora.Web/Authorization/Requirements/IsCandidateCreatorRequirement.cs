using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsCandidateCreatorRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsCandidateCreatorRequirement, Candidate>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCandidateCreatorRequirement requirement, Candidate candidate)
            {
                var candidates = QueryProcessor.Find<Candidate>(new IsCreatedByUserIdSpec(context.User.GetId()));

                if (candidates.Any(x => x.CreatedByUserId == candidate.CreatedByUserId))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
