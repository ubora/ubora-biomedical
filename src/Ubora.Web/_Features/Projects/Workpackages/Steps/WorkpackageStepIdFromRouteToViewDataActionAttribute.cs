using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class WorkpackageStepIdFromRouteToViewDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.ContainsKey("stepId"))
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    var stepIdAsString = context.RouteData.Values["stepId"] as string;
                    controller.ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.Step(stepIdAsString);
                }
            }
        }
    }
}