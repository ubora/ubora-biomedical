using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public sealed class OrSpecification<TEntity> : CompositeSpecification<TEntity>
    {

        public OrSpecification(params Specification<TEntity>[] specifications) : base(specifications)
        {
        }

        protected override Expression<Func<TEntity, bool>> CombineExpressions(Expression<Func<TEntity, bool>> exp1, Expression<Func<TEntity, bool>> exp2)
        {
            return exp1.Or(exp2);
        }
    }
}
