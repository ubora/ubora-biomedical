using System;
using System.Linq;
using Ubora.Domain.ApplicableRegulations.Specifications;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.ApplicableRegulations.Queries
{
    public class ActiveApplicableRegulationsQuestionnaireQuery : IQuery<ApplicableRegulationsQuestionnaireAggregate>
    {
        public Guid ProjectId { get; set; }

        public class Handler : QueryHandler<ActiveApplicableRegulationsQuestionnaireQuery, ApplicableRegulationsQuestionnaireAggregate>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override ApplicableRegulationsQuestionnaireAggregate Handle(ActiveApplicableRegulationsQuestionnaireQuery query)
            {
                var specification = new IsActiveApplicableRegulationsQuestionnaireSpec(query.ProjectId);

                return QueryProcessor.Find(specification).FirstOrDefault();
            }
        }
    }
}