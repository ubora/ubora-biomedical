using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web.Infrastructure
{
    public abstract class BaseTempDataModelStateAttribute : ActionFilterAttribute
    {
        protected string Key => nameof(BaseTempDataModelStateAttribute);
    }

    public class SaveTempDataModelStateAttribute : BaseTempDataModelStateAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var controller = filterContext.Controller as Controller;

            var serializedModelState = GetSerializedModelState(controller);
            if (serializedModelState != null)
            {
                controller.TempData[Key] = serializedModelState;
            }
        }

        private string GetSerializedModelState(Controller controller)
        {
            var modelState = controller?.ViewData.ModelState;
            if (modelState == null || !modelState.Any()) return null;

            var listError = modelState.Where(x => x.Value.Errors.Any())
                .ToDictionary(m => m.Key, m => m.Value.Errors
                .Select(s => s.ErrorMessage)
                .FirstOrDefault(s => s != null));

            return JsonConvert.SerializeObject(listError);
        }
    }

    public class RestoreModelStateFromTempDataAttribute : BaseTempDataModelStateAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var controller = filterContext.Controller as Controller;
            var savedModelState = GetModelStateFromTempData(controller);
            if (savedModelState != null)
            {
                controller.ViewData.ModelState.Merge(savedModelState);
            }
        }

        private ModelStateDictionary GetModelStateFromTempData(Controller controller)
        {
            var tempData = controller?.TempData?.Keys;
            if (controller != null && tempData != null)
            {
                if (tempData.Contains(Key))
                {
                    var modelStateString = controller.TempData[Key].ToString();
                    var listError = JsonConvert.DeserializeObject<Dictionary<string, string>>(modelStateString);
                    var modelState = new ModelStateDictionary();

                    foreach (var item in listError)
                    {
                        modelState.AddModelError(item.Key, item.Value ?? string.Empty);
                    }

                    return modelState;
                }
            }

            return null;
        }
    }
}
