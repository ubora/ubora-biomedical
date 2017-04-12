using Autofac;
using JetBrains.Annotations;

namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IComponentContext _resolver;

        public CommandBus([NotNull]IComponentContext resolver)
        {
            _resolver = resolver;
        }

        public ICommandResult Execute<T>([NotNull]T command) where T : ICommand
        {
            var handler = _resolver.Resolve<ICommandHandler<T>>();

            return handler.Handle(command);
        }
    }
}