using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Web._Features.Projects.History._Base;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkPackageTwoEventViewModel : EditWorkPackageStepEventViewModel, IEventViewModel<WorkpackageTwoStepEdited>
    {
        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/WorkPackages/_EditWorkPackageTwoEventPartial.cshtml", this);
        }

        public class Factory : EventViewModelFactory<WorkpackageTwoStepEdited, EditWorkPackageTwoEventViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override EditWorkPackageTwoEventViewModel Create(WorkpackageTwoStepEdited editEvent, DateTimeOffset timestamp)
            {
                var viewModel = new EditWorkPackageTwoEventViewModel
                {
                    EventInitiatedBy = editEvent.InitiatedBy,
                    StepId = editEvent.StepId,
                    Title = editEvent.Title,
                    Timestamp = timestamp
                };

                return viewModel;
            }
        }
    }
}
