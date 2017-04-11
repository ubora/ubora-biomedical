namespace Ubora.Domain.Commands
{
    public interface IQueryBus
    {
        TResult Query<TResult>(IQuery<TResult> query) where TResult : IQueryResult;
    }
}