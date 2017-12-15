using System;
using System.Linq;
using Autofac;
using Marten;
using Marten.Pagination;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure
{
    public interface ICommandQueryProcessor : ICommandProcessor, IQueryProcessor
    {
    }

    public class CommandQueryProcessor : ICommandQueryProcessor
    {
        private readonly IComponentContext _handlerResolver;
        private readonly IQuerySession _querySession;

        public CommandQueryProcessor(IComponentContext handlerResolver, IQuerySession querySession)
        {
            _handlerResolver = handlerResolver;
            _querySession = querySession;
        }

        public ICommandResult Execute<T>(T command) where T : ICommand
        {
            var handler = _handlerResolver.Resolve<ICommandHandler<T>>();
            var result = handler.Handle(command);
            return result;
        }

        public T ExecuteQuery<T>(IQuery<T> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(T));
            var handler = _handlerResolver.Resolve(handlerType);
            var methodInfo = handlerType.GetMethod("Handle");
            dynamic queryResult = methodInfo.Invoke(handler, new object[]{query });
            return queryResult;
        }

        public IPagedList<T> Find<T>(ISpecification<T> specification)
        {
            return Find(specification, int.MaxValue, 1);
        }

        public IPagedList<T> Find<T>(ISpecification<T> specification, int pageSize, int pageNumber)
        {
            return Find(specification, null, pageSize, pageNumber);
        }

        public virtual IPagedList<TDocument> Find<TDocument>(ISpecification<TDocument> specification, ISortSpecification<TDocument> sortSpecification, int pageSize, int pageNumber)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            var query = _querySession.Query<TDocument>().AsQueryable();
            query = specification.SatisfyEntitiesFrom(query);
            query = sortSpecification?.Sort(query) ?? query;
            return query.AsPagedList(pageNumber, pageSize);
        }

        public IPagedList<TDocumentProjection> Find<TDocument, TDocumentProjection>(ISpecification<TDocument> specification,
            IProjection<TDocument, TDocumentProjection> projection, ISortSpecification<TDocumentProjection> sortSpecification, int pageSize, int pageNumber)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));
            if (projection == null) throw new ArgumentNullException(nameof(projection));
            
            var query = _querySession.Query<TDocument>().AsQueryable();
            query = specification.SatisfyEntitiesFrom(query);
            var queryWitProjection = projection.Apply(query);
            queryWitProjection = sortSpecification?.Sort(queryWitProjection) ?? queryWitProjection;
            return queryWitProjection.AsPagedList(pageNumber, pageSize);
        }


        public T FindById<T>(Guid id)
        {
            return _querySession.Load<T>(id);
        }
    }
}