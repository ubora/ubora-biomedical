using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Infrastructure.Commands
{
    public class CommandResult : ICommandResult
    {
        private readonly List<string> _errorMessages = new List<string>();
        public IEnumerable<string> ErrorMessages => _errorMessages;

        public CommandResult(params string[] errors)
        {
            if (errors != null)
            {
                _errorMessages.AddRange(errors);
                IsFailure = _errorMessages.Any();
            }
        }

        public bool IsFailure { get; private set; }
        public bool IsSuccess => !IsFailure;

        public static CommandResult Success { get; } = new CommandResult();

        public static CommandResult Failed(params string[] errors)
        {
            if (errors == null || !errors.Any())
            {
                throw new ArgumentException("Can not fail without errors.", nameof(errors));
            }
            return new CommandResult(errors);
        }
    }
}