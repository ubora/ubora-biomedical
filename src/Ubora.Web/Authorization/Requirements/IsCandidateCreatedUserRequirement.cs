using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsCandidateCreatedUserRequirement : IAuthorizationRequirement
    {
        public class Handler : ProjectAuthorizationHandler<IsCandidateCreatedUserRequirement, Candidate>
        {
            public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
            {
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCandidateCreatedUserRequirement requirement, Candidate candidate)
            {
                var candidates = QueryProcessor.Find<Candidate>(new HasCreatedByUserId(context.User.GetId()));

                if (candidates.Any(x => x.CreatedByUserId == candidate.CreatedByUserId))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}
