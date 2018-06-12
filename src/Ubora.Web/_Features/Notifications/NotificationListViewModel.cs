using System.Collections.Generic;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web._Features.Notifications
{
    public class NotificationListViewModel
    {
        public Pager Pager { get; set; }
        public IEnumerable<INotificationViewModel> Notifications { get; set; }
    }
}
