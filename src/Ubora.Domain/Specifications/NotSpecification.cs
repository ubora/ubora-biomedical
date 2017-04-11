using System;
using System.Linq.Expressions;

namespace DomainModels.Specifications
{
    public class NotSpecification<TEntity> : Specification<TEntity>
    {
        public Specification<TEntity> Specification { get; private set; }

        public NotSpecification(Specification<TEntity> specification)
        {
            Specification = specification;
        }

        internal sealed override Expression<Func<TEntity, bool>> ToExpression() => Specification.ToExpression().Not();
    }
}
