using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features.Projects.Tasks
{
    [Authorize]
    public class TasksController : ProjectController
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IMapper _mapper;

        public TasksController(ICommandQueryProcessor processor, IMapper mapper)
        {
            _processor = processor;
            _mapper = mapper;
        }

        public IActionResult Tasks(Guid projectId)
        {
            var project = _processor.FindById<Project>(projectId);
            var projectTasks = _processor.Find<ProjectTask>().Where(x => x.ProjectId == projectId);

            var model = new TaskListViewModel
            {
                ProjectId = projectId,
                ProjectName = project.Title,
                Tasks = projectTasks.Select(_mapper.Map<TaskListItemViewModel>)
            };

            return View(model);
        }

        public IActionResult Add(Guid projectId)
        {
            var model = new AddTaskViewModel
            {
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AddTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new AddTaskCommand
            {
                Id = Guid.NewGuid(),
                InitiatedBy = this.UserInfo
            };
            _mapper.Map(model, command);

            _processor.Execute(command);

            return RedirectToAction(nameof(Tasks), new { projectId = model.ProjectId });
        }

        public IActionResult Edit(Guid id)
        {
            var task = _processor.FindById<ProjectTask>(id);

            var model = _mapper.Map<EditTaskViewModel>(task);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new EditTaskCommand
            {
                InitiatedBy = this.UserInfo
            };
            _mapper.Map(model, command);

            _processor.Execute(command);

            return RedirectToAction(nameof(Tasks), new { projectId = model.ProjectId });
        }
    }
}