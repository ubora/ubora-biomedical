namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandProcessor
    {
        ICommandResult Execute<T>(T command) where T : ICommand;
    }
}