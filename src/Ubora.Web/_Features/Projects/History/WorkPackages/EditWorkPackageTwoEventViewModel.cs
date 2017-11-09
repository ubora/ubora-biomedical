using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web._Features.Users;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkpackageTwoEventViewModel : EditWorkpackageStepEventViewModel, IEventViewModel<WorkpackageTwoStepEdited>
    {
        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/WorkPackages/_EditWorkpackageTwoEventPartial.cshtml", this);
        }

        public class Factory : EventViewModelFactory<WorkpackageTwoStepEdited, EditWorkpackageTwoEventViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override EditWorkpackageTwoEventViewModel Create(WorkpackageTwoStepEdited editEvent, DateTimeOffset timestamp)
            {
                var viewModel = new EditWorkpackageTwoEventViewModel
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
