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
        private readonly IMapper _mapper;

        public TasksController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        public IActionResult Tasks(Guid projectId)
        {
            var project = FindById<Project>(projectId);
            var projectTasks = Find<ProjectTask>().Where(x => x.ProjectId == projectId);

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
                UserInfo = this.UserInfo
            };
            _mapper.Map(model, command);

            ExecuteCommand(command);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Tasks), new { projectId = model.ProjectId });
        }

        public IActionResult Edit(Guid id)
        {
            var task = FindById<ProjectTask>(id);

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
                UserInfo = this.UserInfo
            };
            _mapper.Map(model, command);

            ExecuteCommand(command);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Tasks), new { projectId = model.ProjectId });
        }
    }
}