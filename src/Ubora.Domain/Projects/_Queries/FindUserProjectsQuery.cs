using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Domain.Projects._Queries
{
    public class FindUserProjectsQuery : IQuery<IReadOnlyCollection<Project>>
    {
        public Guid UserId { get; set; }

        internal class Handler : IQueryHandler<FindUserProjectsQuery, IReadOnlyCollection<Project>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IReadOnlyCollection<Project> Handle(FindUserProjectsQuery query)
            {
                var projects = _querySession.Query<Project>()
                    .Where(new HasMember(query.UserId));

                return projects.ToArray();
            }
        }
    }
}
