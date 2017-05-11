using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static void ExecuteCommand(this Controller controller, ICommandProcessor commandProcessor, ICommand command)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (commandProcessor == null) throw new ArgumentNullException(nameof(commandProcessor));
            if (command == null) throw new ArgumentNullException(nameof(command));

            var commandResult = commandProcessor.Execute(command);
            if (commandResult.IsFailure)
            {
                controller.ModelState.AddCommandErrors(commandResult);
            }
        }
    }
}