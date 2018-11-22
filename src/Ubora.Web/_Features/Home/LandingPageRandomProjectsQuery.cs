using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Projections;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Web._Features.Home
{
    /// <summary>
    /// Finds projects to display for the landing page. Projects should not be in draft and should have an image.
    /// </summary>
    public class LandingPageRandomProjectsQuery : IQuery<IReadOnlyCollection<Project>>
    {
        public class Handler : IQueryHandler<LandingPageRandomProjectsQuery, IReadOnlyCollection<Project>>
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IQuerySession _querySession;

            public Handler(IQueryProcessor queryProcessor, IQuerySession querySession)
            {
                _queryProcessor = queryProcessor;
                _querySession = querySession;
            }

            public IReadOnlyCollection<Project> Handle(LandingPageRandomProjectsQuery query)
            {
                var randomizedNotDraftProjectIds = _queryProcessor.Find(
                        specification: !new IsDraftSpec() && new HasImageSpec(),
                        sortSpecification: null,
                        projection: new IdProjection(),
                        pageSize: int.MaxValue,
                        pageNumber: 1)
                    .OrderBy(x => Guid.NewGuid()) // randomize order
                    .Take(6)
                    .ToArray();

                var projects = _querySession.Query<Project>().Where(project => project.Id.IsOneOf(randomizedNotDraftProjectIds));

                return projects.ToArray();
            }
        }
    }
}
