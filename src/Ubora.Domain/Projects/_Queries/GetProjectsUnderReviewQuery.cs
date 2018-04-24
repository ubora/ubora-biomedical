using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects._Queries
{
    public class GetProjectsUnderReviewQuery : IQuery<IReadOnlyCollection<Project>>
    {
        internal class Handler : IQueryHandler<GetProjectsUnderReviewQuery, IReadOnlyCollection<Project>>
        {
            private readonly IQuerySession _querySession;
            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IReadOnlyCollection<Project> Handle(GetProjectsUnderReviewQuery query)
            {
                var underReviewProjectIds = 
                    _querySession
                        .Query<WorkpackageOne>()
                        .Where(wp1 => wp1.HasReviewInProcess)
                        .Select(x => x.ProjectId)
                        .ToArray();

                return _querySession.LoadMany<Project>(underReviewProjectIds);
            }
        }
    }
}
