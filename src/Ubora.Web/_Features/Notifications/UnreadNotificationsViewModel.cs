using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Queries;

namespace Ubora.Web._Features.Notifications
{
    public class UnreadNotificationsViewModel
    {
        public int UnreadMessagesCount { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public UnreadNotificationsViewModel Create(Guid currentUserId)
            {
                var unreadMessagesCount = _queryProcessor
                    .ExecuteQuery(new CountQuery<INotification>(
                      new HasUnViewedNotifications(currentUserId)));

                return new UnreadNotificationsViewModel { UnreadMessagesCount = unreadMessagesCount };
            }
        }
    }
}
