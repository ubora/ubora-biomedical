using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.Members.Specifications
{
    public class IsMentorSpec : Specification<UserProfile>
    {
        internal override Expression<Func<UserProfile, bool>> ToExpression()
        {
            return x => x.RoleKey == "project-mentor";
        }
    }
}