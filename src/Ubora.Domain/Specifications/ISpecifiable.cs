using DomainModels.Specifications;

namespace Ubora.Domain.Specifications
{
    public interface ISpecifiable<T> where T : ISpecifiable<T>
    {
        bool DoesSatisfy(ISpecification<T> specification);
    }
}
