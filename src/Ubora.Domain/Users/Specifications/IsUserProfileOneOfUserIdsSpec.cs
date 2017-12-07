using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Marten;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.Specifications
{
    public class IsUserProfileOneOfUserIdsSpec : Specification<UserProfile>
    {
        public Guid[] UserIds { get; }

        public IsUserProfileOneOfUserIdsSpec(IEnumerable<Guid> userIds)
        {
            UserIds = userIds.ToArray();
        }

        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            return x => x.UserId.IsOneOf(UserIds);
        }
    }
}