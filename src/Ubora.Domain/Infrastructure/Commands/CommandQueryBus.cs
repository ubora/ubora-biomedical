using Autofac;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Infrastructure.Commands
{
    // Processor
    public class CommandQueryBus : ICommandBus, IQueryBus
    {
        private readonly IComponentContext _resolver;

        public CommandQueryBus(IComponentContext resolver)
        {
            _resolver = resolver;
        }

        public ICommandResult Execute<T>(T command) where T : ICommand
        {
            var handler = _resolver.Resolve<ICommandHandler<T>>();

            return handler.Handle(command);
        }

        public TResult Find<TResult>(IQuery<TResult> query)
        {
            throw new System.NotImplementedException();
        }

        public T Find<T>(ISpecification<T> specification = null)
        {
            throw new System.NotImplementedException();
        }
    }
}