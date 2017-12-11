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
                var inProcessProjectIds = Queryable.Where<WorkpackageOne>(_querySession.Query<WorkpackageOne>(), x => x.HasReviewInProcess)
                    .Select(x => x.ProjectId);

                var projects = _querySession.LoadMany<Project>(inProcessProjectIds.ToArray());
                return projects;
            }
        }
    }
}
