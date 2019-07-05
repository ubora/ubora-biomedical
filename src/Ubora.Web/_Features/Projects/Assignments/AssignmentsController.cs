using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Assignments.Commands;
using Ubora.Web._Features._Shared.Notices;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Assignments
{
    [ProjectRoute("[controller]")]
    public class AssignmentsController : ProjectController
    {
        public async Task<IActionResult> Assignments([FromServices]AssignmentListViewModel.Factory modelFactory)
        {
            var model = await modelFactory.Create(User, Project);
            return View(model);
        }

        [Route(nameof(Add))]
        public async Task<IActionResult> Add([FromServices]AddAssignmentViewModel.Factory modelFactory)
        {
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, null, Policies.CanWorkOnAssignments)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }
            var model = modelFactory.Create(ProjectId);
            return View(model);
        }

        [HttpPost]
        [Route(nameof(Add))]
        public async Task<IActionResult> Add(AddAssignmentViewModel model, [FromServices]AddAssignmentViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return await Add(modelFactory);
            }
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, null, Policies.CanWorkOnAssignments)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }
            ExecuteUserProjectCommand(new AddAssignmentCommand
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                AssigneeIds = model.AssigneeIds
            }, Notice.Success(SuccessTexts.AssignmentAdded));

            if (!ModelState.IsValid)
            {
                return await Add(modelFactory);
            }

            return RedirectToAction(nameof(Assignments), new { ProjectId });
        }

        [Route(nameof(View))]
        public IActionResult View(Guid id, [FromServices]AssignmentViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(id);
            return View(model);
        }

        [Route(nameof(Edit))]
        public async Task<IActionResult> Edit(Guid id, [FromServices]EditAssignmentViewModel.Factory modelFactory)
        {
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, null, Policies.CanWorkOnAssignments)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }
            var model = modelFactory.Create(id);
            return View(model);
        }

        [HttpPost]
        [Route(nameof(Edit))]
        public async Task<IActionResult> Edit(EditAssignmentViewModel model, [FromServices]EditAssignmentViewModel.Factory modelFactory)
        {
            
            if (!ModelState.IsValid)
            {
                return await Edit(model.Id, modelFactory);
            }
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, null, Policies.CanWorkOnAssignments)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }
            ExecuteUserProjectCommand(new EditAssignmentCommand
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                AssigneeIds = model.AssigneeIds
            }, Notice.Success(SuccessTexts.AssignmentEdited));

            if (!ModelState.IsValid)
            {
                return await Edit(model.Id, modelFactory);
            }

            return RedirectToAction(nameof(Assignments), new { ProjectId });
        }

        [HttpPost]
        [Route(nameof(ToggleAssignmentStatus))]
        public async Task<IActionResult> ToggleAssignmentStatus(string id)
        {
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, null, Policies.CanWorkOnAssignments)).Succeeded;
            if (!isAuthorized)
            {
                return Forbid();
            }
            ExecuteUserProjectCommand(new ToggleAssignmentDoneStatusCommand
            {
                Id = new Guid(id)
            }, Notice.None("for now ?"));
            return RedirectToAction(nameof(Assignments));
        }
    }
}