using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.Projects
{
    public class ProjectTitleToViewDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var projectController = (ProjectController)context.Controller;
            projectController.ViewData["ProjectTitle"] = projectController.Project.Title;
        }
    }
}