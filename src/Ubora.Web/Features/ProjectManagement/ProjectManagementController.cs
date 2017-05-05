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
            // TODO
            return RedirectToAction(nameof(StepTwo), new { id });

            var project = _processor.FindById<Project>(id);

            var model = new DashboardViewModel
            {
                Title = project.Title,
                Id = project.Id,
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

            var model = _mapper.Map<StepOneViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public IActionResult StepOne(StepOneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new UpdateProjectCommand
            {
                UserInfo = this.UserInfo
            };

            var project = _processor.FindById<Project>(model.Id);
            _mapper.Map(project, command);

            command.Title = model.Title;
            command.ClinicalNeedTags = model.ClinicalNeedTags;
            command.AreaOfUsageTags = model.AreaOfUsageTags;
            command.PotentialTechnologyTags = model.PotentialTechnologyTags;
            command.GmdnTerm = model.GmdnTerm;

            _processor.Execute(command);

            return RedirectToAction(nameof(Dashboard), new { id = model.Id });
        }

        public IActionResult StepTwo(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = _mapper.Map<StepTwoViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public IActionResult StepTwo(StepTwoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new UpdateProjectCommand
            {
                UserInfo = this.UserInfo
            };

            var project = _processor.FindById<Project>(model.Id);
            _mapper.Map(project, command);

            command.DescriptionOfNeed = model.DescriptionOfNeed;
            command.DescriptionOfExistingSolutionsAndAnalysis = model.DescriptionOfExistingSolutionsAndAnalysis;
            command.ProductFunctionality = model.ProductFunctionality;
            command.ProductPerformance = model.ProductPerformance;
            command.ProductUsability = model.ProductUsability;
            command.ProductSafety = model.ProductSafety;
            command.PatientPopulationStudy = model.PatientPopulationStudy;
            command.UserRequirementStudy = model.UserRequirementStudy;
            command.AdditionalInformation = model.AdditionalInformation;

            _processor.Execute(command);

            return RedirectToAction(nameof(Dashboard), new { id = model.Id });
        }
    }
}
