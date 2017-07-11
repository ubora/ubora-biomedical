using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class FindByEmailQuery : IQuery<UserProfile>
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        public class Handler : QueryHandler<FindByEmailQuery, UserProfile>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override UserProfile Handle(FindByEmailQuery query)
            {
                var userProfile = QueryProcessor.Find<UserProfile>()
                    .SingleOrDefault(x => x.Email.Equals(query.Email, StringComparison.OrdinalIgnoreCase));

                return userProfile;
            }
        }
    }
}
