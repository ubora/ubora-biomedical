using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public abstract class WrappedSpecification<TEntity> : Specification<TEntity>
    {
        internal abstract Specification<TEntity> WrapSpecifications();

        internal sealed override Expression<Func<TEntity, bool>> ToExpression() => WrapSpecifications().ToExpression();
    }
}
