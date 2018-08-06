using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ubora.Web.Infrastructure
{
    public class CommaSeparatedValuesModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;
            if (!String.IsNullOrEmpty(value))
            {
                var model = value.Split(",");
                int[] ids = Array.ConvertAll(model, int.Parse);
                bindingContext.Result = ModelBindingResult.Success(ids);
            }

            return Task.CompletedTask;
        }
    }
}