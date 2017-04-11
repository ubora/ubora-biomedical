using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface IQueryBus
    {
        TResult Query<TResult>(IQuery<TResult> query) where TResult : IQueryResult;
        T Query<T>(ISpecification<T> specification);
    }
}