namespace Ubora.Domain.Infrastructure.Commands
{
    public interface IQuery<TResult> where TResult : IQueryResult
    {
    }
}