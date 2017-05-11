using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure
{
    public abstract class Entity<TDerived> : ISpecifiable<TDerived> where TDerived : Entity<TDerived>
    {
        public bool DoesSatisfy(ISpecification<TDerived> specification)
        {
            return specification.IsSatisfiedBy((TDerived)this);
        }

        public abstract class Specification : Specification<TDerived>
        {
        }
    }
}