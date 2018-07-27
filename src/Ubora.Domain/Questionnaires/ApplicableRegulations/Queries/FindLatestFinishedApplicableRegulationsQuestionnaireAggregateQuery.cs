using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Queries
{
    public class FindLatestFinishedApplicableRegulationsQuestionnaireAggregateQuery : IQuery<ApplicableRegulationsQuestionnaireAggregate>
    {
        public FindLatestFinishedApplicableRegulationsQuestionnaireAggregateQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

        public Guid ProjectId { get; }

        public class Handler : IQueryHandler<FindLatestFinishedApplicableRegulationsQuestionnaireAggregateQuery, ApplicableRegulationsQuestionnaireAggregate>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public ApplicableRegulationsQuestionnaireAggregate Handle(FindLatestFinishedApplicableRegulationsQuestionnaireAggregateQuery query)
            {
                return _querySession
                    .Query<ApplicableRegulationsQuestionnaireAggregate>()
                    .Where(x => x.ProjectId == query.ProjectId)
                    .Where(x => x.IsFinished)
                    .OrderByDescending(x => x.FinishedAt)
                    .FirstOrDefault();
            }
        }
    }
}
