using System.Collections.Generic;

namespace Ubora.Web._Features.Notifications
{
    public class IndexViewModel
    {
        public List<IndexInvitationViewModel> Invitations { get; set; } = new List<IndexInvitationViewModel>();
    }
}