namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; set; }
    }
}