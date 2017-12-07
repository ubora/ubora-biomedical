using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Projects.Workpackages.Queries
{
    public class IsWorkpackageThreeOpenedQuery : IQuery<bool>
    {
        public Guid ProjectId { get; }

        public IsWorkpackageThreeOpenedQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

        internal class Handler : IQueryHandler<IsWorkpackageThreeOpenedQuery, bool>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public bool Handle(IsWorkpackageThreeOpenedQuery query)
            {
                var workPackagesThreeCount = _querySession.Query<WorkpackageThree>()
                    .Count(x => x.ProjectId == query.ProjectId);

                return workPackagesThreeCount == 1;
            }
        }
    }
}
