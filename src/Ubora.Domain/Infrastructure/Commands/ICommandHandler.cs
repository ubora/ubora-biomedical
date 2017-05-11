using Marten;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }

    internal abstract class CommandHandler<T> : ICommandHandler<T> where T : ICommand
    {
        protected IDocumentSession DocumentSession { get; private set; }

        protected CommandHandler(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
        }

        public abstract ICommandResult Handle(T cmd);
    } 
}