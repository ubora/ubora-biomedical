using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public static JsonResult ToJsonResult(this ModelStateDictionary modelState)
        {
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            return new JsonResult(new
            {
                errors = modelState.Keys.SelectMany(k => modelState[k].Errors)
                    .Select(m => m.ErrorMessage).ToArray()
            });
        }
    }
}