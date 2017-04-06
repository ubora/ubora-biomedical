namespace Ubora.Domain.Commands
{
    public interface ICommandProcessor
    {
        void Execute(ICommand command);
        TResult Execute<TResult>(ICommand<TResult> command) where TResult : ICommandResult;
    }
}