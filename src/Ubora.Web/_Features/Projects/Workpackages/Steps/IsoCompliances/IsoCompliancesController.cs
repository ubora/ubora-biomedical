using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances
{
    [ProjectRoute("WP4/ISO-compliance")]
    public class IsoCompliancesController : ProjectController
    {
        private readonly IndexViewModel.Factory _indexViewModelFactory;

        public IsoStandardsComplianceChecklist IsoStandardsComplianceAggregate { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            IsoStandardsComplianceAggregate = QueryProcessor.FindById<IsoStandardsComplianceChecklist>(ProjectId);
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.IsoCompliance;
        }

        public IsoCompliancesController(IndexViewModel.Factory indexViewModelFactory)
        {
            _indexViewModelFactory = indexViewModelFactory;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index", _indexViewModelFactory.Create(IsoStandardsComplianceAggregate));
        }

        [HttpPost("add-standard")]
        public IActionResult AddIsoStandard(AddIsoStandardPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserProjectCommand(new AddIsoStandardCommand
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Link = new Uri(model.Link)
            }, Notice.Success(SuccessTexts.IsoStandardAdded));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("remove-standard")]
        public IActionResult RemoveIsoStandard(RemoveIsoStandardCommand model)
        {
            if (!AuthorizationService.IsAuthorized(User, model.IsoStandardId, Policies.CanRemoveIsoStandardFromComplianceChecklist))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserProjectCommand(model, Notice.Success(SuccessTexts.IsoStandardRemoved));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("mark-as-compliant")]
        public IActionResult MarkAsCompliant(MarkIsoStandardAsCompliantCommand model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserProjectCommand(model, Notice.Success(SuccessTexts.IsoStandardMarkedAsCompliant));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("mark-as-noncompliant")]
        public IActionResult MarkAsNoncompliant(MarkIsoStandardAsNoncompliantCommand model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            ExecuteUserProjectCommand(model, Notice.Success(SuccessTexts.IsoStandardMarkedAsNonCompliant));

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}