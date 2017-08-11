using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users.Specifications;

namespace Ubora.Domain.Users.Queries
{
    public class SearchUsersQuery : IQuery<IEnumerable<UserProfile>>
    {
        public string SearchPhrase { get; set; }

        public SearchUsersQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public class Handler : QueryHandler<SearchUsersQuery, IEnumerable<UserProfile>>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override IEnumerable<UserProfile> Handle(SearchUsersQuery query)
            {
                var users = QueryProcessor.Find(new UserFullNameContainsPhraseSpec(query.SearchPhrase)
                    || new UserEmailContainsPhraseSpec(query.SearchPhrase));

                return users;
            }
        }
    }
}
