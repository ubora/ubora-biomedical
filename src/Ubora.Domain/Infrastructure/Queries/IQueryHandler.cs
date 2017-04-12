namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IQueryHandler<T, TResult> where T : IQuery<TResult>
    {
        TResult Handle(T query);
    }
}