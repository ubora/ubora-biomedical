using System;
using Ubora.Domain.Projects;
using Ubora.Domain.Queries;

namespace Ubora.Domain.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IResolver _resolver;

        public CommandBus(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Command(ICommand command)
        {
            var handler = _resolver.Resolve<ICommandHandler<CreateProjectCommand>>();
            handler.Handle((dynamic)command);
        }

        public TResult Command<TResult>(ICommand<TResult> command) where TResult : ICommandResult
        {
            throw new NotImplementedException();
        }
    }
}