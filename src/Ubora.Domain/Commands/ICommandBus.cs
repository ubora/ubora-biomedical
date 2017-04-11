namespace Ubora.Domain.Commands
{
    public interface ICommandBus
    {
        void Command(ICommand command);
        TResult Command<TResult>(ICommand<TResult> command) where TResult : ICommandResult;
    }
}