using System;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications.Requests
{
    public class RequestPartialViewModel
    {
        [NotDefault]
        public Guid RequestId { get; set; }
        public string Action { get; set; }
    }
}
