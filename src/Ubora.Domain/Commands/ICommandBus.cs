namespace Ubora.Domain.Commands
{
    public interface ICommandBus
    {
        void Execute(ICommand command);
        TResult Execute<TResult>(ICommand<TResult> command) where TResult : ICommandResult;
    }
}