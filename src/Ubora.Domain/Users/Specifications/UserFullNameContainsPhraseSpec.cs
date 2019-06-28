using Marten.Util;
using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.Specifications
{
    public class UserFullNameContainsPhraseSpec : Specification<UserProfile>
    {
        public string SearchPhrase { get; }

        public UserFullNameContainsPhraseSpec(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            if(string.IsNullOrEmpty(SearchPhrase))
            {
                return user => true;
            }
            return user => user.FullName.Contains(SearchPhrase.ToLower(), StringComparison.OrdinalIgnoreCase);
        }
    }
}