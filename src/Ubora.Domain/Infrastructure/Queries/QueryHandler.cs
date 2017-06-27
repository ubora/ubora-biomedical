namespace Ubora.Domain.Infrastructure.Queries
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected QueryHandler(IQueryProcessor queryProcessor)
        {
            QueryProcessor = queryProcessor;
        }

        protected IQueryProcessor QueryProcessor { get; }

        public abstract TResult Handle(TQuery query);
    }
}