using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Queries
{
    public class FindUserProfilesQuery : IQuery<IReadOnlyCollection<UserProfile>>
    {
        public IEnumerable<Guid> UserIds { get; set; }

        internal class Handler : IQueryHandler<FindUserProfilesQuery, IReadOnlyCollection<UserProfile>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IReadOnlyCollection<UserProfile> Handle(FindUserProfilesQuery query)
            {
                return _querySession.LoadMany<UserProfile>(query.UserIds.ToArray())
                    .ToArray();
            }
        }
    }
}
