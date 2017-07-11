using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features.Projects.Assignments
{
    [ProjectRoute("[controller]")]
    public class AssignmentsController : ProjectController
    {
        public IActionResult Assignments()
        {
            var projectTasks = QueryProcessor.Find<ProjectTask>().Where(x => x.ProjectId == ProjectId);

            var model = new AssignmentListViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Assignments = projectTasks.Select(AutoMapper.Map<AssignmentListItemViewModel>)
            };

            return View(model);
        }

        [Route(nameof(Add))]
        public IActionResult Add()
        {
            var model = new AddAssignmentViewModel
            {
                ProjectId = ProjectId
            };

            return View(model);
        }

        [HttpPost]
        [Route(nameof(Add))]
        public IActionResult Add(AddAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteUserProjectCommand(new AddTaskCommand
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Assignments), new { ProjectId });
        }

        [Route(nameof(Edit))]
        public IActionResult Edit(Guid id)
        {
            var task = QueryProcessor.FindById<ProjectTask>(id);

            var model = AutoMapper.Map<EditAssignmentViewModel>(task);

            return View(model);
        }

        [HttpPost]
        [Route(nameof(Edit))]
        public IActionResult Edit(EditAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteUserProjectCommand(new EditTaskCommand
            {
                Title = model.Title,
                Description = model.Description
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Assignments), new { ProjectId });
        }
    }
}