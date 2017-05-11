using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackagesController : ProjectController
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IMapper _mapper;

        public WorkpackagesController(ICommandQueryProcessor processor, IMapper mapper)
        {
            _processor = processor;
            _mapper = mapper;
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

            this.ExecuteCommand(_processor, command);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(StepOne), new { id = model.Id });
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

            this.ExecuteCommand(_processor, command);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Dashboard), "Dashboard", new { id = model.Id });
        }
    }
}
