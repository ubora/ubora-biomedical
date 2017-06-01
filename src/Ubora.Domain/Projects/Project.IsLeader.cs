using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Projects
{
    public class IsLeader : Specification<Project>
    {
        public Guid UserId { get; }

        public IsLeader(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p => p.Members.Any(m => m.UserId == UserId && m is ProjectLeader);
        }
    }
}
