namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandBus
    {
        ICommandResult Execute<T>(T command) where T : ICommand;
    }
}