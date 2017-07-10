using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features.Projects.Assignments
{
    [ProjectRoute("[controller]")]
    public class AssignmentsController : ProjectController
    {
        private readonly IMapper _mapper;

        public AssignmentsController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        public IActionResult Assignments()
        {
            var projectTasks = Find<ProjectTask>().Where(x => x.ProjectId == ProjectId);

            var model = new AssignmentListViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Assignments = projectTasks.Select(_mapper.Map<AssignmentListItemViewModel>)
            };

            return View(model);
        }

        [Route("Add")]
        public IActionResult Add()
        {
            var model = new AddAssignmentViewModel
            {
                ProjectId = ProjectId
            };

            return View(model);
        }

        [HttpPost]
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

        [Route("Edit")]
        public IActionResult Edit(Guid id)
        {
            var task = FindById<ProjectTask>(id);

            var model = _mapper.Map<EditAssignmentViewModel>(task);

            return View(model);
        }

        [HttpPost]
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