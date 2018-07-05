using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Resources.Queries
{
    public class FindResourceMenuRootQuery : IQuery<ResourcesHierarchy>
    {
        public class Handler : IQueryHandler<FindResourceMenuRootQuery, ResourcesHierarchy>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public ResourcesHierarchy Handle(FindResourceMenuRootQuery query)
            {
                return _querySession.Load<ResourcesHierarchy>(ResourcesHierarchy.SingletonId);
            }
        }
    }
}
