using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Assignments.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Features._Shared.Notices;
using Microsoft.AspNetCore.Authorization;

namespace Ubora.Web._Features.Projects.Assignments
{
    [ProjectRoute("[controller]")]
    [Authorize(Policies.CanWorkOnProjectContent)]
    public class AssignmentsController : ProjectController
    {
        public IActionResult Assignments()
        {
            var projectTasks = QueryProcessor.Find<Assignment>(new IsFromProjectSpec<Assignment> { ProjectId = ProjectId });

            var model = new AssignmentListViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Assignments = projectTasks.Select(AutoMapper.Map<AssignmentListItemViewModel>)
            };

            return View(model);
        }

        [Route(nameof(Add))]
        public IActionResult Add([FromServices]AddAssignmentViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(ProjectId);
            return View(model);
        }

        [HttpPost]
        [Route(nameof(Add))]
        public IActionResult Add(AddAssignmentViewModel model, [FromServices]AddAssignmentViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return Add(modelFactory);
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
                return Add(modelFactory);
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
        public IActionResult Edit(Guid id, [FromServices]EditAssignmentViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(id);
            return View(model);
        }

        [HttpPost]
        [Route(nameof(Edit))]
        public IActionResult Edit(EditAssignmentViewModel model, [FromServices]EditAssignmentViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return Edit(model.Id, modelFactory);
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
                return Edit(model.Id, modelFactory);
            }

            return RedirectToAction(nameof(Assignments), new { ProjectId });
        }
    }
}