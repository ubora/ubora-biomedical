using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandResult : ICommandResult
    {
        public IEnumerable<string> ErrorMessages { get; }

        public CommandResult(params string[] errors)
        {
            ErrorMessages = errors;
        }

        public bool IsFailure => ErrorMessages.Any();
        public bool IsSuccess => !IsFailure;
    }
}