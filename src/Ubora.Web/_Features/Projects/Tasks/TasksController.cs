using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features.Projects.Tasks
{
    public class TasksController : ProjectController
    {
        private readonly IMapper _mapper;

        public TasksController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        [Route("tasks")]
        public IActionResult Tasks()
        {
            var projectTasks = Find<ProjectTask>().Where(x => x.ProjectId == ProjectId);

            var model = new TaskListViewModel
            {
                ProjectId = ProjectId,
                ProjectName = Project.Title,
                Tasks = projectTasks.Select(_mapper.Map<TaskListItemViewModel>)
            };

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new AddTaskViewModel
            {
                ProjectId = ProjectId
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

            return RedirectToAction(nameof(Tasks), new { ProjectId });
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

            return RedirectToAction(nameof(Tasks), new { ProjectId });
        }
    }
}