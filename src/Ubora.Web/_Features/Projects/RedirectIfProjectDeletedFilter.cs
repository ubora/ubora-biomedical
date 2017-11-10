using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Web._Features.Projects.DeleteProject;

namespace Ubora.Web._Features.Projects
{
    public class RedirectIfProjectDeletedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var processor = context.HttpContext.RequestServices.GetService<IQueryProcessor>();
            var projectId = context.RouteData.GetProjectId();
            var project = processor.FindById<Project>(projectId);

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
