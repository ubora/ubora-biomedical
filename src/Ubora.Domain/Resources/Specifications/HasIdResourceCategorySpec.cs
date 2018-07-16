using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Resources.Specifications
{
    public class HasIdResourceCategorySpec : Specification<ResourceCategory>
    {
        public HasIdResourceCategorySpec(Guid resourceCategoryId)
        {
            ResourceCategoryId = resourceCategoryId;
        }

        public Guid ResourceCategoryId { get; }

        internal override Expression<Func<ResourceCategory, bool>> ToExpression()
        {
            return category => category.Id == ResourceCategoryId;
        }
    }
}
