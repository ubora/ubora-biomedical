using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.DeviceClassification.Events;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditDeviceClassificationEventViewModel : IEventViewModel<EditedProjectDeviceClassificationEvent>
    {
        public string CurrentClassification { get; set; }
        public string NewClassification { get; set; }
        public UserInfo EventInitiatedBy { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/WorkPackages/_EditDeviceClassificationEventPartial.cshtml", this);
        }

        public class Factory : EventViewModelFactory<EditedProjectDeviceClassificationEvent, EditDeviceClassificationEventViewModel>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public override EditDeviceClassificationEventViewModel Create(EditedProjectDeviceClassificationEvent editEvent, DateTimeOffset timestamp)
            {
                var viewModel = new EditDeviceClassificationEventViewModel
                {
                    CurrentClassification = editEvent.CurrentClassification?.Text,
                    NewClassification = editEvent.NewClassification.Text,
                    EventInitiatedBy = editEvent.InitiatedBy,
                    Timestamp = timestamp
                };

                return viewModel;
            }
        }
    }
}
