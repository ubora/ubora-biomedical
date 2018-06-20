using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Resources.Queries
{
    public class FindResourcePageBySlugOrIdQuery : IQuery<ResourcePage>
    {
        public FindResourcePageBySlugOrIdQuery(string slugOrIdStringValue)
        {
            SlugOrIdStringValue = slugOrIdStringValue;
        }
        
        public string SlugOrIdStringValue { get; }

        public class Handler : IQueryHandler<FindResourcePageBySlugOrIdQuery, ResourcePage>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }
            
            public ResourcePage Handle(FindResourcePageBySlugOrIdQuery query)
            {
                if (Guid.TryParse(query.SlugOrIdStringValue, out Guid id))
                {
                    return _querySession.Load<ResourcePage>(id);
                }

                return _querySession
                    .Query<ResourcePage>()
                    .AsQueryable()
                    .FirstOrDefault(resourcePage => resourcePage.ActiveSlug.Value == query.SlugOrIdStringValue);
            }
        }
    }
}