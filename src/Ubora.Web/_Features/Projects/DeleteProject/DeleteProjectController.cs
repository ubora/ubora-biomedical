using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Home;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.DeleteProject
{
    [DisableProjectControllerAuthorization]
    [Authorize(Policies.CanDeleteProject)]
    public class DeleteProjectController : ProjectController
    {
       
        [Authorize(Policies.CanDeleteProject)]
        public IActionResult DeleteProject()
        {
            var model = new DeleteProjectViewModel
            {
                Title = Project.Title
            };
           
            return View(model);
        }
        [HttpPost]
        [Authorize(Policies.CanDeleteProject)]
        public IActionResult DeleteProject(DeleteProjectViewModel model)
        {
            ExecuteUserProjectCommand(new DeleteProjectCommand
            {
                ProjectId = Project.Id
            }, Notice.Success(SuccessTexts.ProjectDeleted));

            return RedirectToAction(nameof(HomeController.Index), nameof(Home));
        }

        [AllowAnonymous]
        [DontRedirectIfProjectDeleted]
        public IActionResult Deleted()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("DeletedProject");
        }
    }
}
