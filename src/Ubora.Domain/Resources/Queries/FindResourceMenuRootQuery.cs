using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Resources.Queries
{
    public class FindResourceMenuRootQuery : IQuery<ResourcesMenu>
    {
        public class Handler : IQueryHandler<FindResourceMenuRootQuery, ResourcesMenu>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public ResourcesMenu Handle(FindResourceMenuRootQuery query)
            {
                return _querySession.Load<ResourcesMenu>(ResourcesMenu.SingletonId);
            }
        }
    }
}
