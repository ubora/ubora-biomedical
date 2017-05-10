using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Members
{
    public class HasMember : Specification<Project>
    {
        public Guid UserId { get; }

        public HasMember(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Members.Any(m => m.UserId == UserId);
        }
    }
}
