using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IQueryBus
    {
        TResult Find<TResult>(IQuery<TResult> query);
        T Find<T>(ISpecification<T> specification);
    }
}