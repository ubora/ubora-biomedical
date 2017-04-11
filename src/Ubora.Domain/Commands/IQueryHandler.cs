namespace Ubora.Domain.Commands
{
    public interface IQueryHandler<in T, out TResult> where T : IQuery<TResult> where TResult : IQueryResult
    {
        TResult Handle(T query);
    }
}