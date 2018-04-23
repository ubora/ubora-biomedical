using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsDraftSpec : Specification<Project>
    {
        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.IsInDraft;
        }
    }
}
