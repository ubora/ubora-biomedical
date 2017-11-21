using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Specifications
{
    public class IsActiveApplicableRegulationsQuestionnaireSpec : Specification<ApplicableRegulationsQuestionnaireAggregate>
    {
        public Guid ProjectId { get; }

        public IsActiveApplicableRegulationsQuestionnaireSpec(Guid projectId)
        {
            if (projectId == default(Guid)) { throw new ArgumentException(); }
            ProjectId = projectId;
        }

        internal override Expression<Func<ApplicableRegulationsQuestionnaireAggregate, bool>> ToExpression()
        {
            return x => x.ProjectId == this.ProjectId && x.FinishedAt == null;
        }
    }
}
