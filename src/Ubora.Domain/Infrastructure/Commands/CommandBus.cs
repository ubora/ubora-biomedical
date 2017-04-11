using System;
using JetBrains.Annotations;

namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IResolver _resolver;

        public CommandBus([NotNull]IResolver resolver)
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