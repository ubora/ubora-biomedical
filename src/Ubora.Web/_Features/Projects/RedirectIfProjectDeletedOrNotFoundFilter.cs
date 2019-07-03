using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Web._Features.Projects.DeleteProject;

namespace Ubora.Web._Features.Projects
{
    public class RedirectIfProjectDeletedOrNotFoundAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var project = ((ProjectController)context.Controller).Project;

            if (project == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            if (project.IsDeleted)
            {
                if (context.Filters.Any(filter => filter is DontRedirectIfProjectDeletedAttribute))
                {
                    return;
                }
                context.Result = new RedirectToActionResult(nameof(DeleteProjectController.Deleted), nameof(DeleteProjectController).Replace("Controller", ""), routeValues: null);
            }
        }
    }
}