using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Projects
{
    public class HasMember<T> : Specification<Project> where T : ProjectMember
    {
        public Guid UserId { get; }

        public HasMember(Guid userId)
        {
            UserId = userId;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p => p.Members.Any(m => m.UserId == UserId && m is T);
        }
    }

    public class HasMember : HasMember<ProjectMember>
    {
        public HasMember(Guid userId) : base(userId)
        {
        }
    }
}
