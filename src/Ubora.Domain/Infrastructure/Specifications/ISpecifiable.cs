namespace Ubora.Domain.Infrastructure.Specifications
{
    public interface ISpecifiable<T> where T : ISpecifiable<T>
    {
        bool DoesSatisfy(ISpecification<T> specification);
    }
}
