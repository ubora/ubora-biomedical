using System;
using Ubora.Web._Features.Users;

namespace Ubora.Web._Features.Projects.History.WorkPackages
{
    public class EditWorkpackageStepEventViewModel
    {
        public string StepId { get; set; }
        public string Title { get; set; }
        public UserInfoViewModel EventInitiatedBy { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
