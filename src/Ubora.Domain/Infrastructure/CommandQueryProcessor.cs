using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Marten;
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
            dynamic handler = _handlerResolver.Resolve(handlerType);
            var queryResult = handler.Handle((dynamic)query);
            return queryResult;
        }

        public IEnumerable<T> Find<T>(ISpecification<T> specification)
        {
            var dbSet = _querySession.Query<T>();

            return specification != null
                ? specification.SatisfyEntitiesFrom(dbSet).ToList()
                : dbSet.ToList();
        }

        public T FindById<T>(Guid id)
        {
            return _querySession.Load<T>(id);
        }
    }
}