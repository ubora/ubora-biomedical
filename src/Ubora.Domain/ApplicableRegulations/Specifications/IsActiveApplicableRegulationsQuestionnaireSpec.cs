using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.ApplicableRegulations.Specifications
{
    public class IsActiveApplicableRegulationsQuestionnaireSpec : Specification<ProjectQuestionnaireAggregate>
    {
        public Guid ProjectId { get; }

        public IsActiveApplicableRegulationsQuestionnaireSpec(Guid projectId)
        {
            if (projectId == default(Guid)) { throw new ArgumentException(); }
            ProjectId = projectId;
        }

        internal override Expression<Func<ProjectQuestionnaireAggregate, bool>> ToExpression()
        {
            return x => x.ProjectId == this.ProjectId && x.FinishedAt == null;
        }
    }
}
