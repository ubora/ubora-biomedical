using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications.Specifications;

namespace Ubora.Web._Features.Notifications
{
    public class UnreadNotificationsViewModel
    {
        public int UnreadMessagesCount { get; set; }

        public class Factory
        {
            private IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public UnreadNotificationsViewModel Create(Guid currentUserId)
            {
                var unreadMessagesCount = _queryProcessor.Find(new HasUnViewedNotifications(currentUserId))
                    .Count();

                return new UnreadNotificationsViewModel { UnreadMessagesCount = unreadMessagesCount };
            }
        }
    }
}
