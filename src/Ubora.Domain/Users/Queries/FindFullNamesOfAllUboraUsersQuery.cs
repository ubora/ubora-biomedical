using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class FindFullNamesOfAllUboraUsersQuery : IQuery<IReadOnlyDictionary<Guid, string>>
    {
        internal class Handler : IQueryHandler<FindFullNamesOfAllUboraUsersQuery, IReadOnlyDictionary<Guid, string>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IReadOnlyDictionary<Guid, string> Handle(FindFullNamesOfAllUboraUsersQuery query)
            {
                var usersFullNameMap = _querySession.Query<UserProfile>()
                    .OrderBy(x => x.FullName)
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
