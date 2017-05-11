using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public abstract class Projection<TDerived> : ISpecifiable<TDerived> where TDerived : Projection<TDerived>
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