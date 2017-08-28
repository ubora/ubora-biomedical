using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Users.Specifications
{
    public class HasUserId : Specification<UserProfile>
    {
        public Guid UserId { get; }

        public HasUserId(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            return x => x.UserId == this.UserId;
        }
    }
}
