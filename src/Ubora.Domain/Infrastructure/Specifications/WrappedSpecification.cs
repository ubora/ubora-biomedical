using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public abstract class WrappedSpecification<TEntity> : Specification<TEntity>
    {
        public abstract Specification<TEntity> ToSpecification();

        internal sealed override Expression<Func<TEntity, bool>> ToExpression() => ToSpecification().ToExpression();
    }
}
