using System;
using System.Linq.Expressions;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public abstract class WrappedSpecification<TEntity> : Specification<TEntity>
    {
        public abstract Specification<TEntity> Specification { get; }

        internal sealed override Expression<Func<TEntity, bool>> ToExpression() => Specification.ToExpression();
    }
}
