using DomainModels.Specifications;

namespace Ubora.Domain.Commands
{
    public interface IQueryBus
    {
        TResult Query<TResult>(IQuery<TResult> query) where TResult : IQueryResult;
        T Query<T>(ISpecification<T> specification);
    }
}