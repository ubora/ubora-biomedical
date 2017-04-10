using System;
using Ubora.Domain.Projects;

namespace Ubora.Domain.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IResolver _resolver;

        public CommandProcessor(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Execute(ICommand command)
        {
            var handler = _resolver.Resolve<ICommandHandler<CreateProjectCommand>>();
            handler.Handle((dynamic)command);
        }

        public TResult Execute<TResult>(ICommand<TResult> command) where TResult : ICommandResult
        {
            throw new NotImplementedException();
        }
    }
}