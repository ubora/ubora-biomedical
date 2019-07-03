using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsPotentialTechnologyTagsSpec : Specification<Project>
    {
        public string PotentialTechnologyTags { get; }

        public IsPotentialTechnologyTagsSpec(string potentialTechnologyTags)
        {
            PotentialTechnologyTags = potentialTechnologyTags;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return project => String.Equals(project.PotentialTechnologyTag, PotentialTechnologyTags, StringComparison.OrdinalIgnoreCase);
        }
    }
}
