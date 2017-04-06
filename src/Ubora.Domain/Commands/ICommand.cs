namespace Ubora.Domain.Commands
{
    public interface ICommand
    {
    }

    public interface ICommand<TResult> where TResult : ICommandResult
    {
    }
}