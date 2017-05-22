using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure
{
    public abstract class Entity<TDerived> : ISpecifiable<TDerived> where TDerived : Entity<TDerived>
    {
        // Virtual for unit test purposes.
        public virtual bool DoesSatisfy(ISpecification<TDerived> specification)
        {
            return specification.IsSatisfiedBy((TDerived)this);
        }
    }
}