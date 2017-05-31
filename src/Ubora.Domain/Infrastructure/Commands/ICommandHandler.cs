using Marten;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        ICommandResult Handle(T cmd);
    }

    internal abstract class CommandHandler
    {
        protected CommandHandler(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
        }

        protected IDocumentSession DocumentSession { get; private set; }
    }

    internal abstract class CommandHandler<T> : CommandHandler, ICommandHandler<T> where T : ICommand
    {
        protected CommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public abstract ICommandResult Handle(T cmd);
    }
}