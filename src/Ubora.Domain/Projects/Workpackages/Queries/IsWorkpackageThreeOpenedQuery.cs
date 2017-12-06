using System;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Projects.Workpackages.Queries
{
    public class IsWorkpackageThreeOpenedQuery : IQuery<bool>
    {
        public Guid ProjectId { get; set; }

        internal class Handler : IQueryHandler<IsWorkpackageThreeOpenedQuery, bool>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public bool Handle(IsWorkpackageThreeOpenedQuery query)
            {
                return false;
            }
        }
    }
}
