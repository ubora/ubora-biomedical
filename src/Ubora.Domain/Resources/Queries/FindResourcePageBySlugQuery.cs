using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Resources.Queries
{
    public class FindResourcePageBySlugQuery : IQuery<ResourcePage>
    {
        public FindResourcePageBySlugQuery(string slugValue)
        {
            SlugValue = slugValue;
        }
        
        public string SlugValue { get; }

        public class Handler : IQueryHandler<FindResourcePageBySlugQuery, ResourcePage>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }
            
            public ResourcePage Handle(FindResourcePageBySlugQuery query)
            {
                return _querySession
                    .Query<ResourcePage>()
                    .AsQueryable()
                    .FirstOrDefault(resourcePage => resourcePage.Slug.Value == query.SlugValue);
            }
        }
    }
}