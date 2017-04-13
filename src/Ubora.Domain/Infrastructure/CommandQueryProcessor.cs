using System;
using System.Collections.Generic;
using Autofac;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure
{
    public class CommandQueryProcessor : ICommandProcessor, IQueryProcessor
    {
        private readonly IComponentContext _resolver;

        public CommandQueryProcessor(IComponentContext resolver)
        {
            _resolver = resolver;
        }

        public ICommandResult Execute<T>(T command) where T : ICommand
        {
            var handler = _resolver.Resolve<ICommandHandler<T>>();
            var result = handler.Handle(command);
            return result;
        }

        public T ExecuteQuery<T>(IQuery<T> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(T));
            dynamic handler = _resolver.Resolve(handlerType);
            var queryResult = handler.Handle((dynamic)query);
            return queryResult;
        }

        public IEnumerable<T> Find<T>(ISpecification<T> specification)
        {
            throw new System.NotImplementedException();
        }

        public T FindById<T>(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}