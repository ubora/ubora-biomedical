using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web.Infrastructure
{
    public abstract class ViewModelQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected ViewModelQueryHandler(IQueryProcessor queryProcessor)
        {
            QueryProcessor = queryProcessor;
        }

        protected IQueryProcessor QueryProcessor { get; }

        public abstract TResult Handle(TQuery query);
    }
}
