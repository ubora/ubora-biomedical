using System;
using System.Collections.Generic;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Queries
{
    public class FindApplicableRegulationsQuestionnaireAggregatesByIdsQuery : IQuery<IEnumerable<ApplicableRegulationsQuestionnaireAggregate>>
    {
        public Guid [] QuestionnaireIds { get; set; }

        public class Handler : QueryHandler<FindApplicableRegulationsQuestionnaireAggregatesByIdsQuery,
            IEnumerable<ApplicableRegulationsQuestionnaireAggregate>>
        {
            private readonly IQuerySession _querySession;
            
            public Handler(IQueryProcessor queryProcessor, IQuerySession querySession) : base(queryProcessor)
            {
                _querySession = querySession;
            }

            public override IEnumerable<ApplicableRegulationsQuestionnaireAggregate> Handle(FindApplicableRegulationsQuestionnaireAggregatesByIdsQuery byIdsQuery)
            {
                var applicableRegulationsQuestionnaireAggregates =
                    _querySession.LoadMany<ApplicableRegulationsQuestionnaireAggregate>(byIdsQuery.QuestionnaireIds);

                return applicableRegulationsQuestionnaireAggregates;
            }
        }
    }
}