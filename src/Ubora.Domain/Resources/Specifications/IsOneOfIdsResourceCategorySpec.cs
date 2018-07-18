using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Resources.Specifications
{
    public class IsOneOfIdsResourceCategorySpec : Specification<ResourceCategory>
    {
        public IsOneOfIdsResourceCategorySpec(Guid[] resourceCategoryIds)
        {
            ResourceCategoryIds = resourceCategoryIds;
        }

        public Guid[] ResourceCategoryIds { get; }

        internal override Expression<Func<ResourceCategory, bool>> ToExpression()
        {
            return x => x.Id.IsOneOf(ResourceCategoryIds);
        }
    }
}
