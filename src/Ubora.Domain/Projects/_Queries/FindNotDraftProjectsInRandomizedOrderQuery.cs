using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Projections;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Domain.Projects._Queries
{
    public class FindNotDraftProjectsInRandomizedOrderQuery : IQuery<IReadOnlyCollection<Project>>
    {
        public class Handler : IQueryHandler<FindNotDraftProjectsInRandomizedOrderQuery, IReadOnlyCollection<Project>>
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IQuerySession _querySession;

            public Handler(IQueryProcessor queryProcessor, IQuerySession querySession)
            {
                _queryProcessor = queryProcessor;
                _querySession = querySession;
            }

            public IReadOnlyCollection<Project> Handle(FindNotDraftProjectsInRandomizedOrderQuery query)
            {
                var randomizedNotDraftProjectIds = _queryProcessor.Find(
                        specification: !new IsDraftSpec(),
                        sortSpecification: null,
                        projection: new IdProjection(),
                        pageSize: int.MaxValue,
                        pageNumber: 1)
                    .OrderBy(x => Guid.NewGuid())
                    .ToArray();

                var projects = _querySession.Query<Project>().Where(project => project.Id.IsOneOf(randomizedNotDraftProjectIds));

                return projects.ToArray();
            }
        }
    }
}