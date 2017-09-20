using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web._Features.Users;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkpackageOneEventViewModel : EditWorkpackageStepEventViewModel, IEventViewModel<WorkpackageOneStepEditedEvent>
    {
        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/WorkPackages/_EditWorkpackageOneEventPartial.cshtml", this);
        }

        public class Factory : EventViewModelFactory<WorkpackageOneStepEditedEvent, EditWorkpackageOneEventViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override EditWorkpackageOneEventViewModel Create(WorkpackageOneStepEditedEvent editEvent, DateTimeOffset timestamp)
            {
                var viewModel = new EditWorkpackageOneEventViewModel
                {
                    EventInitiatedBy = new UserInfoViewModel(editEvent.InitiatedBy),
                    StepId = editEvent.StepId,
                    Title = editEvent.Title,
                    Timestamp = timestamp
                };

                return viewModel;
            }
        }
    }
}
