using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsClinicalNeedTagsSpec : Specification<Project>
    {
        public string ClinicalNeedTags { get; }

        public IsClinicalNeedTagsSpec(string clinicalNeedTags)
        {
            ClinicalNeedTags = clinicalNeedTags;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return project => String.Equals(project.ClinicalNeedTag, ClinicalNeedTags, StringComparison.OrdinalIgnoreCase);
        }
    }
}
