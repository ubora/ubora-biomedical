using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Members.Specifications
{
    public class HasUserIdSpec : Specification<ProjectMember>
    {
        public Guid UserId { get; }

        public HasUserIdSpec(Guid userId)
        {
            UserId = userId;
        }
        
        internal override Expression<Func<ProjectMember, bool>> ToExpression()
        {
            return x => x.UserId == UserId;
        }
    }
}