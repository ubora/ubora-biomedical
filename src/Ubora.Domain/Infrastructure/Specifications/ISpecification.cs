using System.Linq;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public interface ISpecification<TEntity>
    { 
        IQueryable<TEntity> SatisfyEntitiesFrom(IQueryable<TEntity> query);
        bool IsSatisfiedBy(TEntity entity);
    }
}