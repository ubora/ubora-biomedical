using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddCommandErrors(this ModelStateDictionary modelState, ICommandResult commandResult)
        {
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));
            if (commandResult == null) throw new ArgumentNullException(nameof(commandResult));

            foreach (var errorMessage in commandResult.ErrorMessages)
            {
                modelState.AddModelError(string.Empty, errorMessage);
            }
        }
    }
}