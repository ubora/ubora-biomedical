using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Candidates.Specifications
{
    public class IsProjectCandidateSpec : Specification<Candidate>
    {
        public Guid ProjectId { get; }

        public IsProjectCandidateSpec(Guid projectId)
        {
            ProjectId = projectId;
        }

        internal override Expression<Func<Candidate, bool>> ToExpression()
        {
            return candidate => candidate.ProjectId == ProjectId;
        }
    }
}
