using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Resources.Specifications
{
    public class IsFileFromResourcePageSpec : Specification<ResourceFile>
    {
        public IsFileFromResourcePageSpec(Guid resourcePageId)
        {
            ResourcePageId = resourcePageId;
        }

        public Guid ResourcePageId { get; }

        internal override Expression<Func<ResourceFile, bool>> ToExpression()
        {
            return resourceFile => resourceFile.ResourcePageId == this.ResourcePageId;
        }
    }
}
