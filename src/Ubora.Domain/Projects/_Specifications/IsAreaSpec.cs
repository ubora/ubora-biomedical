using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsAreaSpec : Specification<Project>
    {
        public string AreaOfUsageTags { get; }

        public IsAreaSpec(string areaOfUsageTags)
        {
            AreaOfUsageTags = areaOfUsageTags;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return project => String.Equals(project.AreaOfUsageTag, AreaOfUsageTags, StringComparison.OrdinalIgnoreCase);
        }
    }
}
