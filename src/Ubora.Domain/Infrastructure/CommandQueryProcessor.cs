using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using Marten;
using Marten.Linq;
using Marten.Pagination;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects;

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
            dynamic handler = _handlerResolver.Resolve(handlerType);
            var queryResult = handler.Handle((dynamic)query);
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

    public abstract class SortSpecification<TEntity, TKey> : ISortSpecification<TEntity>
    {
        public IQueryable<TEntity> Sort(IQueryable<TEntity> query)
        {
            return query.OrderBy(KeySelector);
        }

        internal abstract Expression<Func<TEntity, TKey>> KeySelector { get; }
    }

    /// <summary>
    /// Do not use this interface directly for implementing specifications, use abstract SortSpecification<TEntity, Tkey> class for that.
    /// </summary>
    public interface ISortSpecification<TEntity>
    {
        IQueryable<TEntity> Sort(IQueryable<TEntity> query);
    }

    public interface IProjection<TDocument, TDocumentProjection>
    {
        IQueryable<TDocumentProjection> Apply(IQueryable<TDocument> query);
    }

    public abstract class Projection<TDocument, TDocumentProjection> : IProjection<TDocument, TDocumentProjection>
    {
        public IQueryable<TDocumentProjection> Apply(IQueryable<TDocument> query)
        {
            throw new NotImplementedException();
        }

        protected abstract Expression<Func<TDocument, TDocumentProjection>> Select { get; }
    }
}