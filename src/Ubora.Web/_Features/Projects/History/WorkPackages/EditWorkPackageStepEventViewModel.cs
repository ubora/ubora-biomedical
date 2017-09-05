using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkPackageStepEventViewModel
    {
        public string StepId { get; set; }
        public string Title { get; set; }
        public UserInfo EventInitiatedBy { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
