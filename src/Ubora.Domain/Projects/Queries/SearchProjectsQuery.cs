using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Projects.Queries
{
    public class SearchProjectsQuery : IQuery<IEnumerable<Project>>
    {
        public string Title { get; }

        public SearchProjectsQuery(string title)
        {
            Title = title;
        }

        public class Handler : QueryHandler<SearchProjectsQuery, IEnumerable<Project>>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override IEnumerable<Project> Handle(SearchProjectsQuery query)
            {
                var users = QueryProcessor.Find(new TitleContains(query.Title));

                return users;
            }
        }
    }
}
