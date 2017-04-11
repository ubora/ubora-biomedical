using System.Linq;

namespace DomainModels.Specifications
{
    public interface ISpecification<TEntity>
    { 
        IQueryable<TEntity> SatisfyEntitiesFrom(IQueryable<TEntity> query);
        bool IsSatisfiedBy(TEntity entity);
    }
}