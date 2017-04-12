using Autofac;
using JetBrains.Annotations;

namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IComponentContext _resolver;

        public CommandBus(IComponentContext resolver)
        {
            _resolver = resolver;
        }

        public ICommandResult Execute<T>(T command) where T : ICommand
        {
            var handler = _resolver.Resolve<ICommandHandler<T>>();

            return handler.Handle(command);
        }
    }
}