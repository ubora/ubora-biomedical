using Marten.Util;
using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.Specifications
{
    public class UserEmailContainsPhraseSpec : Specification<UserProfile>
    {
        public string SearchPhrase { get; }

        public UserEmailContainsPhraseSpec(string searchPhrase)
        {
            SearchPhrase = searchPhrase.ToLower();
        }

        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            return user => user.Email.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase);
        }
    }
}
