using System;
using System.Collections.Generic;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Queries
{
    public class FindApplicableRegulationsQuestionnaireAggregatesQuery : IQuery<IEnumerable<ApplicableRegulationsQuestionnaireAggregate>>
    {
        public Guid [] QuestionnaireIds { get; set; }

        public class Handler : QueryHandler<FindApplicableRegulationsQuestionnaireAggregatesQuery,
            IEnumerable<ApplicableRegulationsQuestionnaireAggregate>>
        {
            private readonly IQuerySession _querySession;
            
            public Handler(IQueryProcessor queryProcessor, IQuerySession querySession) : base(queryProcessor)
            {
                _querySession = querySession;
            }

            public override IEnumerable<ApplicableRegulationsQuestionnaireAggregate> Handle(FindApplicableRegulationsQuestionnaireAggregatesQuery query)
            {
                var applicableRegulationsQuestionnaireAggregates =
                    _querySession.LoadMany<ApplicableRegulationsQuestionnaireAggregate>(query.QuestionnaireIds);

                return applicableRegulationsQuestionnaireAggregates;
            }
        }
    }
}