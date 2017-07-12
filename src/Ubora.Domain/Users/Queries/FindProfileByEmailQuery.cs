using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class FindProfileByEmailQuery : IQuery<UserProfile>
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        public class Handler : QueryHandler<FindProfileByEmailQuery, UserProfile>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override UserProfile Handle(FindProfileByEmailQuery query)
            {
                var userProfile = QueryProcessor.Find<UserProfile>()
                    .SingleOrDefault(x => x.Email.Equals(query.Email, StringComparison.OrdinalIgnoreCase));

                return userProfile;
            }
        }
    }
}
