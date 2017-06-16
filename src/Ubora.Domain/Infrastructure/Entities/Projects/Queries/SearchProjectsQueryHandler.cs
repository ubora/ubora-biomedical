using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Infrastructure.Entities.Projects.Queries
{
    public abstract class SearchProjectsQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected SearchProjectsQueryHandler(IQueryProcessor queryProcessor)
        {
            QueryProcessor = queryProcessor;
        }

        protected IQueryProcessor QueryProcessor { get; }

        public abstract TResult Handle(TQuery query);
    }
}