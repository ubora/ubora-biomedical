using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web.Features.ProjectManagement.Tasks
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IMapper _mapper;

        public TasksController(ICommandQueryProcessor processor, IMapper mapper)
        {
            _processor = processor;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public IActionResult List()
        {
            var tasks = _processor.Find<ProjectTask>();

            var model = new TaskListViewModel
            {
                Tasks = tasks.Select(t => _mapper.Map<TaskListItemViewModel>(t))
            };

            return View(model);
        }

        public IActionResult Add(Guid projectId)
        {
            return View(new AddTaskViewModel { ProjectId = projectId });
        }

        [HttpPost]
        public IActionResult Add(AddTaskViewModel model)
        {
            var taskId = Guid.NewGuid();
            // Todo
            var command = new AddTaskCommand { TaskId = taskId, InitiatedBy = new UserInfo(Guid.NewGuid(), "")};
            _mapper.Map(model, command);

            _processor.Execute(command);

            return RedirectToAction(nameof(List));
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


            return RedirectToAction(nameof(List));
        }
    }

    public class TaskListViewModel
    {
        public IEnumerable<TaskListItemViewModel> Tasks { get; set; }
    }

    public class TaskListItemViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class AddTaskViewModel
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class EditTaskViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}