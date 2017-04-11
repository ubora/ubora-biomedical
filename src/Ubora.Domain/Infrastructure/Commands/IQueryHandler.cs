namespace Ubora.Domain.Infrastructure.Commands
{
    public interface IQueryHandler<T, TResult> where T : IQuery<TResult> where TResult : IQueryResult
    {
        TResult Handle(T query);
    }
}