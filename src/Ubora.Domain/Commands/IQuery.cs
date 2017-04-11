namespace Ubora.Domain.Commands
{
    public interface IQuery<TResult> where TResult : IQueryResult
    {
    }
}