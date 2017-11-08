using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Home;

namespace Ubora.Web._Features.Projects.DeleteProject
{
    [DisableProjectControllerAuthorization]
    [Authorize(Policies.CanDeleteProject)]
    public class DeleteProjectController : ProjectController
    {
       
        [Authorize(Policies.CanDeleteProject)]
        public IActionResult DeleteProject()
        {
            //   await _storageProvider.DeleteBlobAsync(BlobLocation.ContainerNames.Projects, ProjectId.ToString());
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
            //   await _storageProvider.DeleteBlobAsync(BlobLocation.ContainerNames.Projects, ProjectId.ToString());

            ExecuteUserProjectCommand(new DeleteProjectCommand
            {
                ProjectId = Project.Id,
            });

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
