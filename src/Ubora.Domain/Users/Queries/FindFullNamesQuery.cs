using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Queries
{
    public class FindFullNamesQuery : IQuery<IReadOnlyDictionary<Guid, string>>
    {
        public FindFullNamesQuery(IEnumerable<Guid> userIds)
        {
            UserIds = userIds.ToArray();
        }

        public Guid[] UserIds { get; set; }

        internal class Handler : IQueryHandler<FindFullNamesQuery, IReadOnlyDictionary<Guid, string>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IReadOnlyDictionary<Guid, string> Handle(FindFullNamesQuery query)
            {
                var usersFullNameMap = _querySession.Query<UserProfile>()
                    .Where(userProfile => userProfile.UserId.IsOneOf(query.UserIds))
                    .Select(userProfile => new
                    {
                        UserId = userProfile.UserId,
                        FullName = userProfile.FullName
                    })
                    .ToDictionary(x => x.UserId, x => x.FullName);

                return usersFullNameMap;
            }
        }
    }
}
