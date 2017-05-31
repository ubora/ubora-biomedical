using System.Collections.Generic;

namespace Ubora.Web._Features.Notifications
{
    public class HistoryViewModel
    {
        public List<HistoryInvitationViewModel> Invitations { get; set; } = new List<HistoryInvitationViewModel>();
    }

    public class HistoryInvitationViewModel
    {
        public string ProjectTitle { get; set; }
        public bool WasAccepted { get; set; }
    }
}
