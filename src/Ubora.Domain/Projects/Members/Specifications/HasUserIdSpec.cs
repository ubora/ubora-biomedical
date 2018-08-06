using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Members.Specifications
{
    public class HasUserIdSpec : Specification<UserProfile>
    {
        public Guid UserId { get; }

        public HasUserIdSpec(Guid userId)
        {
            UserId = userId;
        }
        
        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            return x => x.UserId == UserId;
        }
    }
}