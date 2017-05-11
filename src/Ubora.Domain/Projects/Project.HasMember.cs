using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Projects
{
    public class HasMember : Project.Specification
    {
        public Guid UserId { get; }

        public HasMember(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return HasMember(m => m.UserId == UserId);
        }
    }
}
