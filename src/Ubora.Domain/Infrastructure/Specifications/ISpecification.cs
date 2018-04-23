using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public interface ISpecification<TEntity>
    { 
        IQueryable<TEntity> SatisfyEntitiesFrom(IQueryable<TEntity> query);
        IEnumerable<TEntity> SatisfyEntitiesFrom(IEnumerable<TEntity> enumerable);
        bool IsSatisfiedBy(TEntity entity);
    }
}