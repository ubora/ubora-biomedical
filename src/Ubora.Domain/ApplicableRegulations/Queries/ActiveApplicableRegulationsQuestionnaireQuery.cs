using System;
using System.Linq;
using Ubora.Domain.ApplicableRegulations.Specifications;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.ApplicableRegulations.Queries
{
    public class ActiveApplicableRegulationsQuestionnaireQuery : IQuery<ProjectQuestionnaireAggregate>
    {
        public Guid ProjectId { get; set; }

        public class Handler : QueryHandler<ActiveApplicableRegulationsQuestionnaireQuery, ProjectQuestionnaireAggregate>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override ProjectQuestionnaireAggregate Handle(ActiveApplicableRegulationsQuestionnaireQuery query)
            {
                var specification = new IsActiveApplicableRegulationsQuestionnaireSpec(query.ProjectId);

                return QueryProcessor.Find(specification).FirstOrDefault();
            }
        }
    }
}