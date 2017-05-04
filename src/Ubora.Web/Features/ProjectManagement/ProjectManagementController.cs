using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.ProjectManagement
{
    [Authorize]
    public class ProjectManagementController : ControllerBase
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IEventStreamQuery _eventStreamQuery;
        private readonly IMapper _mapper;

        public ProjectManagementController(ICommandQueryProcessor processor, IEventStreamQuery eventStreamQuery, IMapper mapper)
        {
            _processor = processor;
            _eventStreamQuery = eventStreamQuery;
            _mapper = mapper;
        }

        public IActionResult Index(Guid id)
        {
            return RedirectToAction(nameof(Dashboard), new { id });
        }

        public IActionResult Dashboard(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = new DashboardViewModel
            {
                Name = project.Title,
                Id = project.Id,
                Description = project.Description
            };

            return View(model);
        }

        public IActionResult History(Guid id)
        {
            var projectEventStream = _eventStreamQuery.Find(id);

            var model = new HistoryViewModel
            {
                Events = projectEventStream.Select(x => x.ToString())
            };

            return View(model);
        }

        public IActionResult StepOne(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = _mapper.Map<UpdateProjectViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStepOne(UpdateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new UpdateProjectCommand { UserInfo = this.UserInfo };
            _mapper.Map(model, command);

            return RedirectToAction(nameof(Dashboard), new { id = model.Id });
        }

        public IActionResult StepTwo(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = _mapper.Map<UpdateProjectViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStepTwo(UpdateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new UpdateProjectCommand { UserInfo = this.UserInfo };
            _mapper.Map(model, command);

            _processor.Execute(command);

            return RedirectToAction(nameof(Dashboard), new { id = model.Id });
        }
    }

    public class UpdateProjectViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string DescriptionOfNeed { get; set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; set; }
        public string ProductPerformance { get; set; }
        public string ProductUsability { get; set; }
        public string ProductSafety { get; set; }
        public string PatientsTargetGroup { get; set; }
        public string EndusersTargetGroup { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
