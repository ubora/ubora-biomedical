using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Web._Features.Projects.History._Base;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkPackageOneEventViewModel : IEventViewModel<WorkpackageOneStepEditedEvent>
    {
        public string StepId { get; set; }
        public string Title { get; set; }
        public UserInfo EventInitiatedBy { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/WorkPackages/_EditWorkPackageOneEventPartial.cshtml", this);
        }

        public class Factory : EventViewModelFactory<WorkpackageOneStepEditedEvent, EditWorkPackageOneEventViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override EditWorkPackageOneEventViewModel Create(WorkpackageOneStepEditedEvent editEvent, DateTimeOffset timestamp)
            {
                var viewModel = new EditWorkPackageOneEventViewModel
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
