using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.Specifications
{
    public class IsOfAnyUserId : Specification<UserProfile>
    {
        public IEnumerable<Guid> UserIds { get; }

        public IsOfAnyUserId(IEnumerable<Guid> userIds)
        {
            UserIds = userIds;
        }

        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            // TODO(Kaspar Kallas): SQL not yet supported: https://github.com/JasperFx/marten/issues/784
            return x => this.UserIds.Contains(x.UserId);
        }
    }
}