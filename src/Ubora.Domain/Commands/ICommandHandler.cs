namespace Ubora.Domain.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        void Handle(T command);
    }

    public interface ICommandHandler<in T, out TResult> where T : ICommand where TResult : ICommandResult
    {
        TResult Handle(T command);
    }
}