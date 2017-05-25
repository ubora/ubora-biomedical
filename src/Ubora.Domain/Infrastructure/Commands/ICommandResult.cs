using System.Collections.Generic;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandResult
    {
        IEnumerable<string> ErrorMessages { get; }
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }
}