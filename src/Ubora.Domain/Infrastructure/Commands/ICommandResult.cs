namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandResult
    {
        bool IsSuccess { get; }
    }
}